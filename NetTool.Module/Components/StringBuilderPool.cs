using System.Collections.Concurrent;
using System.Text;

namespace NetTool.Module.Components;

public class StringBuilderPool
{
    private const int MaxQueueSize = 50;

    private readonly ConcurrentQueue<StringBuilder> _queue = new();
    private int _count;

    public StringBuilder Rent()
    {
        if (_queue.TryDequeue(out var sender))
        {
            Interlocked.Decrement(ref _count);
            return sender;
        }

        return new StringBuilder();
    }

    public void Return(StringBuilder builder)
    {
        if (Interlocked.Increment(ref _count) > MaxQueueSize)
        {
            Interlocked.Decrement(ref _count);
            return;
        }

        builder.Clear();
        _queue.Enqueue(builder);
    }
}