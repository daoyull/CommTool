using NetTool.Lib.Interface;
using System.Net.Sockets;

namespace NetTool.Module.Messages;

public readonly struct TcpServerMessage : IMessage
{
    public DateTime Time { get; } = DateTime.Now;

    public TcpServerMessage(Socket client, byte[] data)
    {
        Client = client;
        Data = data;
    }

    public Socket Client { get; }
    public byte[] Data { get; }
}