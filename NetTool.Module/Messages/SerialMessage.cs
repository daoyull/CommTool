using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public readonly struct SerialPortMessage(byte[] data) : IMessage
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime Time { get; } = DateTime.Now;
    
    /// <summary>
    /// 接收数据
    /// </summary>
    public byte[] Data { get; } = data;
    
}