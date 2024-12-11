using NetTool.Lib.Args;


namespace NetTool.Lib.Interface;

public interface ICommunication<T> : IDisposable where T : IMessage
{
    /// <summary>
    /// 是否连接
    /// </summary>
    public bool IsConnect { get; }

    /// <summary>
    /// 关闭事件
    /// </summary>
    event EventHandler<ClosedArgs>? Closed;

    /// <summary>
    /// 连接事件
    /// </summary>
    event EventHandler<ConnectedArgs>? Connected;

    /// <summary>
    /// Ui展示区
    /// </summary>
    public INetUi Ui { get; set; } 

    /// <summary>
    /// 连接选项
    /// </summary>
    public IConnectOption ConnectOption { get; }

    /// <summary>
    /// 接收选项
    /// </summary>
    public IReceiveOption ReceiveOption { get; }
    
    /// <summary>
    /// 发送选项
    /// </summary>
    public ISendOption SendOption { get; }

    /// <summary>
    /// 读取收到的消息
    /// </summary>
    ValueTask<T> MessageReadAsync(CancellationToken token);

    /// <summary>
    /// 写入数据
    /// </summary>
    public void Write(byte[] buffer, int offset, int count);

    /// <summary>
    /// 异步写入数据
    /// </summary>
    public Task WriteAsync(byte[] buffer, int offset, int count);

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close();
    
    /// <summary>
    /// 打开连接
    /// </summary>
    public void Connect();
}