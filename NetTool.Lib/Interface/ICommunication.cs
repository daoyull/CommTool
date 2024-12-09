using System.Collections.ObjectModel;
using System.Net.Sockets;
using NetTool.Lib.Args;
using NetTool.Lib.Entity;

namespace NetTool.Lib.Interface;

public interface ICommunication<T> : IDisposable where T : IMessage
{
    public bool IsConnect { get; }

    #region 事件

    event EventHandler<ClosedArgs>? Closed;

    event EventHandler<ConnectedArgs>? Connected;

    #endregion

    public IReceiveOption ReceiveOption { get; }
    public ISendOption SendOption { get; }

    ValueTask<T> MessageReadAsync(CancellationToken token);

    public void Write(byte[] buffer, int offset, int count);

    public Task WriteAsync(byte[] buffer, int offset, int count);
    
    public void Close();
}

public interface ISerialPort
{
    public ISerialConnectOption SerialConnectOption { get; }

    public ISerialReceiveOption SerialReceiveOption { get; }
    public ISerialSendOption SerialSendOption { get; }

    public void Connect();

    

    public List<string> GetPortNames();
}

public interface ITcpClient
{
    public ITcpClientOption TcpClientOption { get; }
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    public ITcpClientSendOption TcpClientSendOption { get; }

    public Task ConnectAsync();
    
}

public interface ITcpServer
{
    public ITcpServerOption TcpServerOption { get; }

    public ITcpServerReceiveOption TcpServerReceiveOption { get; }

    public ITcpServerSendOption TcpServerSendOption { get; }

    public void Listen();

    event EventHandler<Socket>? ClientConnected;
    
    event EventHandler<Socket>? ClientClosed;

    public void Write(Socket socket, byte[] buffer, int offset, int count);
    public Task WriteAsync(Socket socket, byte[] buffer, int offset, int count);
}