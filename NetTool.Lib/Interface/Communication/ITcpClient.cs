namespace NetTool.Lib.Interface;

public interface ITcpClient
{
    public ITcpClientConnectOption TcpClientConnectOption { get; }
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    public ITcpClientSendOption TcpClientSendOption { get; }

    public Task ConnectAsync();
}