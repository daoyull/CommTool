using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public readonly struct UdpMessage(byte[] data) : IMessage
{
    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; } = data;
}