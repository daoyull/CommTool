using System.Collections.Concurrent;
using NetTool.Lib.Messages;

namespace NetTool.Lib.Common;

public class ReceiveMessagePool
{
    private const int MaxQueueSize = 500;

    private readonly ConcurrentQueue<ReceiveMessage> _queue = new();
    private int _count;

    public ReceiveMessage Rent()
    {
        if (_queue.TryDequeue(out var sender))
        {
            Interlocked.Decrement(ref _count);
            return sender;
        }
        
        return new ReceiveMessage();
    }

    public void Return(ReceiveMessage message)
    {
        if (Interlocked.Increment(ref _count) > MaxQueueSize)
        {
            Interlocked.Decrement(ref _count);
            return;
        }
        message.Reset();
        _queue.Enqueue(message);
    }
}