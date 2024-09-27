using System.Diagnostics;
using System.Threading.Channels;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;

namespace NetTool.Lib.Abstracts;

public abstract class AbstractCommunication<T> : ICommunication<T> where T : IMessage
{
    public INotify Notify { get; }

    public IUiLogger? UiLogger { get; set; }
    public IGlobalOption GlobalOption { get; }

    public AbstractCommunication(INotify notify, IGlobalOption globalOption)
    {
        Notify = notify;
        GlobalOption = globalOption;
    }


    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false
    });

    public bool IsConnect { get; protected set; }
    public event EventHandler<ClosedArgs>? Closed;

    public event EventHandler<ConnectedArgs>? Connected;
    public abstract IReceiveOption ReceiveOption { get; }
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

    public IAsyncEnumerable<T> MessageReadAsync() => _channel.Reader.ReadAllAsync();

    public abstract void Write(byte[] buffer, int offset, int count);

    public virtual Task WriteAsync(byte[] buffer, int offset, int count)
    {
        return Task.Factory.StartNew(() => Write(buffer, offset, count));
    }

    protected ValueTask WriteMessage(T t)
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