using System.IO;

namespace Comm.WPF.Common;

public static class FileLog
{
    private static Queue<(string, string)> _queue = new();

    public static void WriteMessage(string type, string content)
    {
        _queue.Enqueue((type, content));
    }

    static string LogPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "logs");

    static FileLog()
    {
        // 创建日志文件夹
        if (Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }

        Task.Factory.StartNew(WriteToFile, CancellationToken.None, TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
    }

    private static async Task WriteToFile()
    {
        while (true)
        {
            if (_queue.Count == 0)
            {
                await Task.Delay(300);
                continue;
            }

            var log = _queue.Dequeue();
            var logPath = Path.Combine(LogPath, log.Item1);
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            var logFile = Path.Combine(logPath, $"{DateTime.Now:yyyyMMdd}.log");
            await File.AppendAllTextAsync(logFile, log.Item2 + Environment.NewLine);
        }
    }
}