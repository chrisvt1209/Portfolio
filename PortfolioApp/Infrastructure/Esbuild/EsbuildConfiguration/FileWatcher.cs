using System.Diagnostics;
using System.Text;

namespace PortfolioApp.Infrastructure.Esbuild.EsbuildConfiguration;

public class FileWatcher : IDisposable
{
    private static FileWatcher? _instance;
    private string Wwwroot { get; set; }
    private List<FileSystemWatcher> Watchers { get; set; }
    public bool HasNewError => Errors.Count > 0;
    public bool HasChanges => Changes.Count > 0;

    private static readonly Dictionary<string, DateTime> LastEventTimes = [];
    private static readonly object BuildLock = new();

    private List<string> Errors { get; set; } = [];
    private List<string> Changes { get; set; } = [];

    public static FileWatcher GetInstance(string path)
    {
        return _instance ??= new FileWatcher(path);
    }

    private FileWatcher(string path)
    {
        Wwwroot = path;
        Watchers = [];
    }

    public void Init()
    {
        Console.WriteLine($"Initializing FileWatcher");
        string[] filters = { "*.js", "*.scss", "*.css" };
        foreach (string filter in filters)
        {
            FileSystemWatcher watcher = new FileSystemWatcher($@"{Wwwroot}\assets")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.FileName,
                Filter = filter,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Error += OnError;

            Watchers.Add(watcher);
        }
    }

    public List<string> GetAllErrors()
    {
        List<string> errors = Errors;
        Errors = [];
        return errors;
    }

    public bool HasPendingChanges(out List<string> changes)
    {
        changes = Changes;
        Changes = [];
        return changes.Count > 0;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.FullPath.StartsWith($@"{Wwwroot}\bundles"))
        {
            return;
        }

        if (e.Name?.StartsWith("-") == true || e.Name?.ToUpper().EndsWith(".TMP") == true)
        {
            return;
        }

        Console.WriteLine(e.ChangeType);
        const int debounceTimeMs = 500;
        DateTime currentTime = DateTime.Now;

        lock (LastEventTimes)
        {
            if (LastEventTimes.TryGetValue(e.FullPath, out DateTime lastTime) && (currentTime - lastTime).TotalMilliseconds < debounceTimeMs)
            {
                return;
            }

            LastEventTimes[e.FullPath] = currentTime;
        }
        RunBuildProcess();
        Console.WriteLine($"{e.Name} triggered");
        Changes.Add(e.FullPath);
    }

    private void RunBuildProcess()
    {
        lock (BuildLock)
        {
            Process cmd = new Process
            {
                StartInfo =
                {
                    FileName = "node.exe",
                    Arguments = "build.debug.js",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            cmd.Start();
            cmd.WaitForExit();

            string message = cmd.StandardError.ReadToEnd();
            Console.WriteLine(message);

            if (message.Contains("[ERROR]"))
            {
                Errors.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
            }
        }
    }

    private static void OnError(object sender, ErrorEventArgs e)
    {
        PrintException(e.GetException());
    }

    private static void PrintException(Exception? ex)
    {
        if (ex == null)
        {
            return;
        }

        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine("Stacktrace:");
        Console.WriteLine(ex.StackTrace);
        Console.WriteLine();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
