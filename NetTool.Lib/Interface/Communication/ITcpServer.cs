using System.Net.Sockets;

namespace NetTool.Lib.Interface;

public interface ITcpServer
{
    public ITcpServerConnectOption TcpServerConnectOption { get; }

    public ITcpServerReceiveOption TcpServerReceiveOption { get; }

    public ITcpServerSendOption TcpServerSendOption { get; }

    public void Listen();

    event EventHandler<Socket>? ClientConnected;

    event EventHandler<Socket>? ClientClosed;

    public void Write(Socket socket, byte[] buffer, int offset, int count);
    public Task WriteAsync(Socket socket, byte[] buffer, int offset, int count);
}