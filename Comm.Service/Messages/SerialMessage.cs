using Comm.Lib.Interface;

namespace Comm.Service.Messages;

/// <summary>
/// 串口信息
/// </summary>
public readonly struct SerialMessage(byte[] data) : IMessage
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