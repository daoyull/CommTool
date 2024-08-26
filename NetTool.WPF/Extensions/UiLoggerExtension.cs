using NetTool.WPF.Components;

namespace NetTool.WPF.Extensions;

public static class UiLoggerExtension
{
    public static void TimeInfo(this NetLogger logger, string message)
    {
        logger.Info($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]# {message}\n");
    }
}