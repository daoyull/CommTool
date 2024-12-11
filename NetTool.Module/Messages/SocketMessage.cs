using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public readonly struct SocketMessage(byte[] data, string remoteIp) : IMessage
{
    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; } = data;
    public string RemoteIp { get;} = remoteIp;
}