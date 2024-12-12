using System.Buffers;
using System.IO.Pipelines;
using System.IO.Ports;
using System.Net.Sockets;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;

namespace Comm.Service.Service;

public class SerialReceiveTask(
    SerialPort serialPort,
    ICommunication<SerialMessage> communication,
    CancellationTokenSource cts) : AbstractPipeReceiveTask<SerialMessage>(communication, cts)
{
    public SerialPort SerialPort { get; } = serialPort;

    public override Task StartHandle()
    {
        // 启动接收和消费任务
        Task.Run(ReceiveTask, Cts.Token);
        Task.Run(HandleTask, Cts.Token);
        return Task.CompletedTask;
    }
    

    private async Task ReceiveTask()
    {
        try
        {
            const int minimumBufferSize = 4096;
            byte[] memory = new byte[minimumBufferSize];
            while (!Cts.IsCancellationRequested)
            {
                if (SerialPort.BytesToRead <= 0)
                {
                    await Task.Delay(1);
                    continue;
                }

                var read = SerialPort.Read(memory, 0, minimumBufferSize);
                await Writer.WriteAsync(memory.AsSpan().Slice(0, read).ToArray());
                FlushResult result = await Writer.FlushAsync();
                if (result.IsCompleted)
                {
                    break;
                }
            }

            await Writer.CompleteAsync();
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
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

                if (message != null && Communication is AbstractCommunication<SerialMessage> serialCommunication)
                {
                    await serialCommunication.WriteMessageAsync(message.Value);
                }
            }
        }
        catch (OperationCanceledException e)
        {
            Console.WriteLine(e);
        }
    }

    private SerialMessage? HandleMaxByteSize(int maxSize)
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

        return new SerialMessage(array);
    }

    private readonly List<byte> _list = new();
    private SerialMessage? HandleMaxTime(int maxTime)
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

        return new SerialMessage(_list.ToArray());
    }
    
}