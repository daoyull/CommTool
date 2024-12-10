namespace NetTool.Lib.Interface;

/// <summary>
/// 接收到的消息
/// </summary>
public interface IMessage
{
    /// <summary>
    /// 消息接收时间
    /// </summary>
    public DateTime Time { get; }

    /// <summary>
    /// 接收数据
    /// </summary>
    public byte[] Data { get; }
}