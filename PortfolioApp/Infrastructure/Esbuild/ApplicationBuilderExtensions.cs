using Microsoft.AspNetCore.Mvc.Razor;
using PortfolioApp.Infrastructure.Esbuild.EsbuildConfiguration;
using PortfolioApp.Infrastructure.Esbuild.TagHelpers;
using System.Collections.Concurrent;

namespace PortfolioApp.Infrastructure.Esbuild;

public static class ApplicationBuilderExtensions
{
    public static IServiceCollection ConfigureEsBuild(this IServiceCollection services, bool isDevelopment)
    {
        services.AddSingleton<ITagHelperInitializer<EsbuildTagHelper>>(new EsbuildTagHelperInitializer(isDevelopment));
        return services;
    }

    public static void UseEsbuild(this IApplicationBuilder app, string webRoothPath)
    {
        FileWatcher fileWatcher = FileWatcher.GetInstance(webRoothPath);
        fileWatcher.Init();
        ConcurrentDictionary<HttpContext, StreamWriter> connections = new();
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.ToString().Equals("/sse"))
            {
                HttpResponse response = context.Response;
                response.Headers.Append("Content-Type", "text/event-stream");
                response.Headers.Append("Cache-Control", "no-cache");
                response.Headers.Append("Connection", "keep-alive");

                StreamWriter streamWriter = new StreamWriter(response.Body);
                connections[context] = streamWriter;

                CancellationToken cancellationToken = context.RequestAborted;

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (fileWatcher.HasNewError)
                        {
                            List<string> currentErrors = fileWatcher.GetAllErrors();
                            foreach (string error in currentErrors)
                            {
                                foreach (StreamWriter connection in connections.Values)
                                {
                                    await connection.WriteAsync($"data: {error}\n\n");
                                    await connection.FlushAsync(cancellationToken);
                                }
                            }
                        }

                        if (fileWatcher.HasPendingChanges(out _))
                        {
                            foreach (StreamWriter connection in connections.Values)
                            {
                                await connection.WriteAsync("data: refresh\n\n");
                                await connection.FlushAsync(cancellationToken);
                            }
                        }


                        foreach (StreamWriter connection in connections.Values)
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
