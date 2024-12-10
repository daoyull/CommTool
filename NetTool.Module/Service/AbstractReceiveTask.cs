using System.Diagnostics;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public abstract class AbstractReceiveTask(IReceiveOption receiveOption, CancellationTokenSource cts) : IReceiveTask
{
    private readonly Stopwatch _stopwatch = new();
    private readonly List<byte> _list = new();

    protected abstract bool IsBreakConnect { get; }

    public abstract int CanReadByte { get; }
    public IReceiveOption ReceiveOption { get; } = receiveOption;
    public event EventHandler<byte[]>? FrameReceive;

    public abstract int Read(byte[] buffer, int size);

    public CancellationTokenSource Cts { get; } = cts;
    public Action? CloseEvent { get; set; }

    public async Task StartTask()
    {
        try
        {
            while (Cts is { IsCancellationRequested: false })
            {
                if (!IsBreakConnect)
                {
                    return;
                }
                if (CanReadByte == 0)
                {
                    await Task.Delay(1);
                    continue;
                }

                byte[] buffer;
                // 判断截取方式
                if (ReceiveOption.MaxFrameSize > 0)
                {
                    // 按照最大包长
                    buffer = await HandleMaxByteSize(ReceiveOption.MaxFrameSize);
                }
                else
                {
                    // 按照最大时间
                    buffer = await HandleMaxTime(ReceiveOption.MaxFrameTime);
                }

                FrameReceive?.Invoke(this, buffer);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    /// <summary>
    /// 按照最大包长处理
    /// </summary>
    /// <param name="maxFrameSize"></param>
    private Task<byte[]> HandleMaxByteSize(int maxFrameSize)
    {
        int readSize = maxFrameSize >= CanReadByte ? CanReadByte : maxFrameSize;
        byte[] buffer = new byte[readSize];
        Read(buffer, readSize);
        return Task.FromResult(buffer);
    }

    /// <summary>
    /// 按照最大间隔时间处理
    /// </summary>
    /// <param name="maxTime"></param>
    private Task<byte[]> HandleMaxTime(int maxTime)
    {
        // 重置
        _list.Clear();
        byte[] buffer = new byte[2048];
        _stopwatch.Restart();
        
        while (CanReadByte != 0 && _stopwatch.ElapsedMilliseconds <= maxTime)
        {
            var read = Read(buffer, 2048);
            _list.AddRange(buffer.AsSpan().Slice(0, read));
            _stopwatch.Restart();
        }

        var array = _list.ToArray();
        _list.Clear();
        return Task.FromResult(array);
    }
}