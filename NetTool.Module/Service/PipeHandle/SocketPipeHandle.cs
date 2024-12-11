using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;
using NetTool.Module.Share;

namespace NetTool.Module.Service;

public class SocketPipeHandle(ICommunication<SocketMessage> communication, Socket socket, CancellationTokenSource cts)
    : AbstractPipeHandle<SocketMessage>(communication, cts)
{
    public Socket Socket { get; } = socket;
    public override  Task StartHandle()
    {
        // 启动接收和消费任务
        Task.Run(ReceiveTask, Cts.Token);
        Task.Run(HandleTask, Cts.Token);
        return Task.CompletedTask;
    }

    public event EventHandler<Socket>? CloseEvent;

    private async Task ReceiveTask()
    {
        try
        {
            const int minimumBufferSize = 1024;

            while (!Cts.IsCancellationRequested)
            {
                Memory<byte> memory = Writer.GetMemory(minimumBufferSize);
                int bytesRead = await Socket.ReceiveAsync(memory, SocketFlags.None);
                if (bytesRead == 0)
                {
                    OnCloseEvent(Socket);
                    return;
                }

                Writer.Advance(bytesRead);

                FlushResult result = await Writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await Writer.CompleteAsync();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task HandleTask()
    {
        try
        {
            while (!Cts.IsCancellationRequested)
            {
                var readResult = Reader.TryRead(out var result);
                Reader.AdvanceTo(result.Buffer.Start);
                if (!readResult)
                {
                    await Task.Delay(1);
                    continue;
                }

                SocketMessage? message;
                // 判断截取方式
                message = ReceiveOption.IsMaxFrameTime
                    ?  HandleMaxTime(ReceiveOption.MaxFrameTime)
                    :  HandleMaxByteSize(ReceiveOption.MaxFrameSize);

                if (message !=null && Communication is AbstractCommunication<SocketMessage> socketCommunication)
                {
                    await socketCommunication.WriteMessage(message.Value);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private  SocketMessage? HandleMaxByteSize(int maxSize)
    {
        var canRead = Reader.TryRead(out var result);
        var buffer = result.Buffer;
        if (!canRead)
        {
            return null;
        }
        
        var array = buffer.Length > maxSize ?
            buffer.Slice(0, maxSize).ToArray() 
            : buffer.Slice(0, buffer.Length).ToArray();
        var consumed = buffer.GetPosition(array.Length);
        Reader.AdvanceTo(consumed);
        return new SocketMessage(array, Socket.ToRemoteIpStr());
    }

    private readonly List<byte> _list = new();
    private SocketMessage? HandleMaxTime(int maxTime)
    {
        Stopwatch.Restart();
        _list.Clear();
        while (Stopwatch.ElapsedMilliseconds <= maxTime)
        {
            var canRead = Reader.TryRead(out var result);
            var buffer = result.Buffer;
            if (!canRead)
            {
                Reader.AdvanceTo(buffer.Start);
                break;
            }
            ReadOnlySequence<byte> item = buffer.Slice(0, buffer.Length);
            _list.AddRange(item.ToArray());
            Reader.AdvanceTo(item.End);
            Stopwatch.Restart();
        }

        return new SocketMessage(_list.ToArray(), Socket.ToRemoteIpStr());
    }

    protected virtual void OnCloseEvent(Socket e)
    {
        CloseEvent?.Invoke(this, e);
    }
}