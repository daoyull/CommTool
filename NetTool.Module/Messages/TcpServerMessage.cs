using NetTool.Lib.Interface;
using System.Net.Sockets;

namespace NetTool.Module.Messages;

public readonly struct TcpServerMessage : IMessage
{
    public DateTime Time { get; } = DateTime.Now;

    public string RemoteIp { get; }

    public TcpServerMessage(Socket client, byte[] data)
    {
        Data = data;
        RemoteIp = client.RemoteEndPoint?.ToString() ?? "Unknown";
    }

    public byte[] Data { get; }
}