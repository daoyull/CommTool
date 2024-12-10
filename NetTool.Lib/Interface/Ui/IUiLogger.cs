namespace NetTool.Lib.Interface;

/// <summary>
/// 界面日志
/// </summary>
public interface IUiLogger
{
    /// <summary>
    /// 输出消息到界面
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="color">颜色</param>
    public void Write(string message, string color);

    /// <summary>
    /// 普通消息
    /// </summary>
    /// <param name="message"></param>
    public void Info(string message);

    /// <summary>
    /// 主要消息
    /// </summary>
    /// <param name="message"></param>
    public void Primary(string message);

    /// <summary>
    /// 成功消息
    /// </summary>
    /// <param name="message"></param>
    public void Success(string message);

    /// <summary>
    /// 警告消息
    /// </summary>
    /// <param name="message"></param>
    public void Warning(string message);

    /// <summary>
    /// 错误消息
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message);

    /// <summary>
    /// 清除界面日志
    /// </summary>
    public void ClearArea();

    /// <summary>
    /// 更新界面消息后的Action
    /// </summary>
    public Action TickUpdate { get; set; }
}