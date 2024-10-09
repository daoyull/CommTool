using System.Diagnostics;
using System.Net.Sockets;
using NetTool.Lib.Interface;

namespace NetTool.Module.IO;

public class ReceiveSocket
{
    public IGlobalOption GlobalOption { get; }

    public ReceiveSocket(Socket socket, Action<byte[]> handle, Action closeHandle, ISocketReceiveOption option,
        IGlobalOption globalOption, CancellationTokenSource cts)
    {
        GlobalOption = globalOption;
        _socket = socket;
        _handle = handle;
        _closeHandle = closeHandle;
        _option = option;
        _cts = cts;
    }


    private readonly Socket _socket;
    private readonly Action<byte[]> _handle;
    private readonly Action _closeHandle;
    private readonly ISocketReceiveOption _option;
    private readonly CancellationTokenSource _cts;
    private readonly Stopwatch _stopwatch = new();
    private readonly List<byte> _list = new();

    public void ReceiveTask()
    {
        try
        {
            byte[] buffer = new byte[GlobalOption.BufferSize];
            while (_cts is { IsCancellationRequested: false })
            {
                if (_stopwatch.IsRunning &&
                    (_stopwatch.ElapsedMilliseconds > _option.AutoBreakFrameTime ||
                     _socket.Available == 0))
                {
                    var array = _list.ToArray();
                    Console.WriteLine(array.Length);
                    _list.Clear();
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    _handle.Invoke(array);
                }


                if (buffer.Length != GlobalOption.BufferSize)
                {
                    buffer = new byte[GlobalOption.BufferSize];
                }

                var count = _socket.Receive(buffer);
                if (count == 0)
                {
                    _closeHandle.Invoke();
                    return;
                }

                if (!_option.AutoBreakFrame)
                {
                    _handle.Invoke(buffer[..count]);
                    continue;
                }

                if (_socket.Available == 0)
                {
                    if (_stopwatch.IsRunning)
                    {
                        _list.AddRange(buffer[..count]);
                    }
                    else
                    {
                        _handle.Invoke(buffer[..count]);
                    }
                }
                else
                {
                    if (!_stopwatch.IsRunning)
                    {
                        _stopwatch.Start();
                    }

                    _list.AddRange(buffer[..count]);
                }
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
    }
}