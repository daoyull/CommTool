using System.Threading.Channels;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;

namespace NetTool.Lib.Abstracts;

public abstract class AbstractCommunication<T> : ICommunication<T> where T : IMessage
{
    public AbstractCommunication()
    {
        
    }

    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false
    });

    public event EventHandler<ClosedArgs>? Closed;

    public event EventHandler<ConnectedArgs>? Connected;

  

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