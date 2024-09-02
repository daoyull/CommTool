using NetTool.Components;

namespace NetTool.Extensions;

public static class UiLoggerExtension
{
    public static void TimeInfo(this NetLogger logger, string message)
    {
        logger.Info($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]# {message}\n");
    }
}