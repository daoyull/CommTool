using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public class TcpClientMessage: IMessage
{
    public TcpClientMessage(byte[] data)
    {
        Data = data;
    }

    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; }
}