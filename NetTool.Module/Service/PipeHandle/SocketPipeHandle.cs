using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;

namespace NetTool.Module.Service;

public class SocketPipeHandle(ICommunication<SocketMessage> communication, Socket socket, CancellationTokenSource cts)
    : AbstractPipeHandle<SocketMessage>(communication, cts)
{
    public override async Task StartHandle()
    {
        // 接收线程
        Task writing = ReceiveTask();
        Task handle = HandleTask();
        // 处理线程
        await Task.WhenAll(writing, handle);
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
                try
                {
                    int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        OnCloseEvent(socket);
                        return;
                    }

                    Writer.Advance(bytesRead);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                FlushResult result = await Writer.FlushAsync();

                if (result.IsCompleted)
                {
                    break;
                }
            }

            await Writer.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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

                SocketMessage message;
                // 判断截取方式
                message = ReceiveOption.IsMaxFrameTime
                    ? await HandleMaxTime(ReceiveOption.MaxFrameTime)
                    : await HandleMaxByteSize(ReceiveOption.MaxFrameSize);

                if (Communication is AbstractCommunication<SocketMessage> socketCommunication)
                {
                    await socketCommunication.WriteMessage(message);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<SocketMessage> HandleMaxByteSize(int maxSize)
    {
        Reader.TryRead(out var result);
        ReadOnlySequence<byte> line;
        var buffer = result.Buffer;
        if (buffer.Length > maxSize)
        {
            line = buffer.Slice(0, maxSize);
        }
        else
        {
            line = buffer.Slice(0, buffer.Length);
        }

        buffer = buffer.Slice(line.Length, buffer.Length - line.Length);
        Reader.AdvanceTo(line.End);
        return new SocketMessage(line.ToArray(), "");
    }

    private async Task<SocketMessage> HandleMaxTime(int maxTime)
    {
        Stopwatch.Restart();
        var list = new List<byte>();
        while (Stopwatch.ElapsedMilliseconds <= maxTime && Reader.TryRead(out var result))
        {
            var buffer = result.Buffer;
            ReadOnlySequence<byte> item = buffer.Slice(0, buffer.Length);
            list.AddRange(item.ToArray());
            Reader.AdvanceTo(item.End);
            Stopwatch.Restart();
        }

        return new SocketMessage(list.ToArray(), socket.RemoteEndPoint?.ToString() ?? "Undefine");
    }

    protected virtual void OnCloseEvent(Socket e)
    {
        CloseEvent?.Invoke(this, e);
    }
}