using NetTool.Lib.Args;

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

    IAsyncEnumerable<T> MessageReadAsync();

    public void Write(byte[] buffer, int offset, int count);

    public Task WriteAsync(byte[] buffer, int offset, int count);
}

public interface ISerialPort
{
    public ISerialOption SerialOption { get; }

    public ISerialReceiveOption SerialReceiveOption { get; }
    public ISerialSendOption SerialSendOption { get; }

    public void Connect();

    public void Close();

    public List<string> GetPortNames();
}

public interface ITcpClient
{
    public ITcpClientOption TcpClientOption { get; }
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    public ITcpClientSendOption TcpClientSendOption { get; }

    public Task ConnectAsync();

    public void Close();
}

public interface ITcpServer
{
    public ITcpServerOption TcpServerOption { get; }

    public ITcpServerReceiveOption TcpServerReceiveOption { get; }

    public ITcpServerSendOption TcpServerSendOption { get; }
}