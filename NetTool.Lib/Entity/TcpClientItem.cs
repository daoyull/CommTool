using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace NetTool.Lib.Entity;

public class TcpClientItem
{
    public TcpClientItem(Socket socket)
    {
        Socket = socket;
    }

    public Socket Socket { get; set; }

    public EndPoint? EndPoint => Socket.RemoteEndPoint;

    public bool IsPrimary { get; set; }
    public Stopwatch StopWatch { get; } = new();
    public List<byte> List { get; } = new();
}