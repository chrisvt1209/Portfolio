using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.Razor;
using Portfolio.Infrastructure.Esbuild;

namespace Portfolio.Infrastructure.Extensions;

public static class EsbuildExtensions
{
    public static IServiceCollection ConfigureEsBuild(this IServiceCollection services, bool isDevelopment)
    {
        services.AddSingleton<ITagHelperInitializer<EsbuildTagHelper>>(new EsbuildTagHelperInitializer(isDevelopment));
        return services;
    }

    public static void UseEsbuild(this IApplicationBuilder app, string webRootPath)
    {
        var fileWatcher = FileWatcher.GetInstance(webRootPath);
        fileWatcher.Init();
        var connections = new ConcurrentDictionary<HttpContext, StreamWriter>();
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.ToString().Equals("/sse"))
            {
                var response = context.Response;
                response.Headers.Append("Content-Type", "text/event-stream");
                response.Headers.Append("Cache-Control", "no-cache");
                response.Headers.Append("Connection", "keep-alive");

                var streamWriter = new StreamWriter(response.Body);
                connections[context] = streamWriter;

                var cancellationToken = context.RequestAborted;

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (fileWatcher.HasNewError)
                        {
                            var currentErrors = fileWatcher.GetAllErrors();
                            foreach (var error in currentErrors)
                            {
                                foreach (var connection in connections.Values)
                                {
                                    await connection.WriteAsync($"data: {error}\n\n");
                                    await connection.FlushAsync(cancellationToken);
                                }
                            }
                        }

                        if (fileWatcher.HasPendingChanges(out _))
                        {
                            foreach (var connection in connections.Values)
                            {
                                await connection.WriteAsync("data: refresh\n\n");
                                await connection.FlushAsync(cancellationToken);
                            }
                        }


                        foreach (var connection in connections.Values)
                        {
                            await connection.WriteAsync(": heartbeat\n\n");
                            await connection.FlushAsync(cancellationToken);
                        }

                        // Send a heartbeat every 30 seconds to keep the connection alive
                        await Task.Delay(250, cancellationToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("SSE connection closed by client");
                }
                finally
                {
                    connections.TryRemove(context, out _);
                }

                return;
            }

            await next.Invoke();
        });
    }
}