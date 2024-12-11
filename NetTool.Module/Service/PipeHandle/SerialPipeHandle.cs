using System.Buffers;
using System.IO.Pipelines;
using System.IO.Ports;
using System.Net.Sockets;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;

namespace NetTool.Module.Service;

public class SerialPipeHandle(
    SerialPort serialPort,
    ICommunication<SerialPortMessage> communication,
    CancellationTokenSource cts) : AbstractPipeHandle<SerialPortMessage>(communication, cts)
{
    public SerialPort SerialPort { get; } = serialPort;

    public async override Task StartHandle()
    {
        // 接收线程
        Task writing = ReceiveTask();
        Task handle = HandleTask();
        // 处理线程
        await Task.WhenAll(writing, handle);
    }

    public event EventHandler<SerialPort>? CloseEvent;

    private async Task ReceiveTask()
    {
        try
        {
            const int minimumBufferSize = 1024;

            while (!Cts.IsCancellationRequested)
            {
                byte[] memory = new byte[minimumBufferSize];
                try
                {
                    if (serialPort.BytesToRead <= 0)
                    {
                        await Task.Delay(1);
                        continue;
                    }

                    var read = SerialPort.Read(memory, 0, memory.Length);

                    await Writer.WriteAsync(memory.AsSpan().Slice(0, read).ToArray());
                    // Writer.Advance(read);
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

                var message =
                    ReceiveOption.IsMaxFrameTime
                    ? HandleMaxTime(ReceiveOption.MaxFrameTime)
                    : HandleMaxByteSize(ReceiveOption.MaxFrameSize);

                if (message != null && Communication is AbstractCommunication<SerialPortMessage> socketCommunication)
                {
                    await socketCommunication.WriteMessage(message.Value);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private SerialPortMessage? HandleMaxByteSize(int maxSize)
    {
        var canRead = Reader.TryRead(out var result);
        var buffer = result.Buffer;
        if (!canRead)
        {
            return null;
        }

        var array = buffer.Length > maxSize
            ? buffer.Slice(0, maxSize).ToArray()
            : buffer.Slice(0, buffer.Length).ToArray();

        var consumed = buffer.GetPosition(array.Length);
        Reader.AdvanceTo(consumed);

        return new SerialPortMessage(array);
    }

    private SerialPortMessage? HandleMaxTime(int maxTime)
    {
        Stopwatch.Restart();
        var list = new List<byte>();
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
            list.AddRange(item.ToArray());
            Reader.AdvanceTo(item.End);
            Stopwatch.Restart();
        }

        return new SerialPortMessage(list.ToArray());
    }

    protected virtual void OnCloseEvent(SerialPort e)
    {
        CloseEvent?.Invoke(this, e);
    }
}