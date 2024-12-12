namespace Comm.Lib.Interface;

/// <summary>
/// 消息通知接口
/// </summary>
public interface INotify
{
    /// <summary>
    /// 普通消息
    /// </summary>
    /// <param name="message"></param>
    void Info(string message);

    /// <summary>
    /// 成功消息
    /// </summary>
    /// <param name="message"></param>
    void Success(string message);

    /// <summary>
    /// 警告消息
    /// </summary>
    /// <param name="message"></param>
    void Warning(string message);

    /// <summary>
    /// 错误消息
    /// </summary>
    /// <param name="message"></param>
    void Error(string message);
}