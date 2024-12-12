using System.Threading.Channels;
using Common.Lib.Ioc;
using Comm.Lib.Args;
using Comm.Lib.Interface;

namespace Comm.Service.IO;

public abstract class AbstractCommunication<T> : ICommunication<T> where T : IMessage
{
    /// <summary>
    /// 连接成功的 CancellationTokenSource
    /// 关闭连接后取消
    /// </summary>
    protected CancellationTokenSource? Cts { get; set; }

    /// <summary>
    /// 消息通知接口
    /// </summary>
    protected INotify Notify => Ioc.Resolve<INotify>();

    /// <summary>
    /// 全局选项
    /// </summary>
    public IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();

    /// <summary>
    /// 消息 Channel
    /// </summary>
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false
    });

    /// <summary>
    /// 是否连接
    /// </summary>
    public bool IsConnect { get; private set; }

    /// <summary>
    /// 关闭事件
    /// </summary>
    public event EventHandler<ClosedArgs>? Closed;

    /// <summary>
    /// 连接事件
    /// </summary>
    public event EventHandler<ConnectedArgs>? Connected;

    /// <summary>
    /// Ui资源
    /// </summary>
    public ICommUi Ui { get; set; } = null!;

    /// <summary>
    /// 连接选项
    /// </summary>
    public abstract IConnectOption ConnectOption { get; }

    /// <summary>
    /// 接收选项
    /// </summary>
    public abstract IReceiveOption ReceiveOption { get; }

    /// <summary>
    /// 发送选项
    /// </summary>
    public abstract ISendOption SendOption { get; }


    protected void OnClosed(ClosedArgs args)
    {
        IsConnect = false;
        Closed?.Invoke(this, args);
    }

    protected void OnConnected(ConnectedArgs args)
    {
        IsConnect = true;
        Connected?.Invoke(this, args);
    }

    /// <summary>
    /// 读取消息
    /// </summary>
    public ValueTask<T> MessageReadAsync(CancellationToken token) => _channel.Reader.ReadAsync(token);

    /// <summary>
    /// 写入数据
    /// </summary>
    public abstract void Write(byte[] buffer, int offset, int count);

    /// <summary>
    /// 异步写入数据
    /// </summary>
    public virtual Task WriteAsync(byte[] buffer, int offset, int count)
    {
        return Task.Factory.StartNew(() => Write(buffer, offset, count));
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// 打开连接
    /// </summary>
    public abstract void Connect();

    /// <summary>
    /// 向Channel中写入信息
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    internal ValueTask WriteMessageAsync(T t)
    {
        return _channel.Writer.WriteAsync(t);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool isDispose)
    {
        if (isDispose)
        {
        }
    }
}