using System.Net;
using System.Net.Sockets;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Lib.Messages;

namespace NetTool.Module.Service;

public class TcpListenerNet : INetService
{
    private Socket? _server;
    
    private TcpListener _listener;


    public void Dispose()
    {
        _server?.Dispose();
    }

    public event EventHandler<ReceiveArgs>? Received;
    public event EventHandler<ClosedArgs>? Closed;
    public event EventHandler<ConnectedArgs>? Connected;
    public Action<ReceiveMessage> ReceiveMessageAction { get; set; }

    public void StartListen()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
        _server = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        
        
    }
    public bool IsConnect { get; }
}