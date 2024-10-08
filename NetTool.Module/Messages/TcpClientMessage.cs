using NetTool.Lib.Interface;
using NetTool.Module.Share;

namespace NetTool.Module.Messages;

public readonly struct TcpClientMessage(byte[] data) : IMessage
{
    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; } = data;
    
}