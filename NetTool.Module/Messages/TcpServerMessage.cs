using NetTool.Lib.Interface;
using System.Net.Sockets;

namespace NetTool.Module.Messages;

public readonly struct TcpServerMessage : IMessage
{
    public DateTime Time { get; } = DateTime.Now;

    public TcpServerMessage(WeakReference<Socket> client, byte[] data)
    {
        Client = client;
        Data = data;
    }

    WeakReference<Socket> Client { get; }
    public byte[] Data { get; }
    public void ReceiveDisplay(INetUi ui)
    {
        throw new NotImplementedException();
    }

    public void SendDisplay(INetUi ui)
    {
        throw new NotImplementedException();
    }
}