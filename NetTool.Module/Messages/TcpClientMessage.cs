using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public readonly struct TcpClientMessage(byte[] data) : IMessage
{
    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; } = data;

    public void ReceiveDisplay(INetUi ui)
    {
        throw new NotImplementedException();
    }

    public void SendDisplay(INetUi ui)
    {
        throw new NotImplementedException();
    }
}