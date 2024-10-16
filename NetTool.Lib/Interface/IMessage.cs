namespace NetTool.Lib.Interface;

public interface IMessage
{
    /// <summary>
    /// 消息接收时间
    /// </summary>
    public DateTime Time { get; }

    public byte[] Data { get; }
}