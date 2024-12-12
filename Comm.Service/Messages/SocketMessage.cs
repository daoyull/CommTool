using Comm.Lib.Interface;

namespace Comm.Service.Messages;

public readonly struct SocketMessage(byte[] data, string remoteIp) : IMessage
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime Time { get; } = DateTime.Now;
    
    /// <summary>
    /// 接收数据
    /// </summary>
    public byte[] Data { get; } = data;
    
    /// <summary>
    /// 接收数据地址
    /// </summary>
    public string RemoteIp { get;} = remoteIp;
}