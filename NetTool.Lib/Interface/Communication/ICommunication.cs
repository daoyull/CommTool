using NetTool.Lib.Args;


namespace NetTool.Lib.Interface;

public interface ICommunication<T> : IDisposable where T : IMessage
{
    public bool IsConnect { get; }

    #region 事件

    event EventHandler<ClosedArgs>? Closed;

    event EventHandler<ConnectedArgs>? Connected;

    #endregion

    public IConnectOption ConnectOption { get;  }

    public IReceiveOption ReceiveOption { get; }
    public ISendOption SendOption { get; }

    ValueTask<T> MessageReadAsync(CancellationToken token);

    public void Write(byte[] buffer, int offset, int count);

    public Task WriteAsync(byte[] buffer, int offset, int count);

    public void Close();
    public void Connect();
}