using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using NetTool.Lib.Args;
using NetTool.Lib.Common;
using NetTool.Lib.Interface;
using NetTool.Lib.Messages;

namespace NetTool.Module.Service;

public class TcpClientNet : ITcpClientService
{
    public int ReceiveBufferSize { get; set; } = 256;

    public long AutoBreakTime { get; set; } = 20;
    public bool AutoBreak { get; set; } = true;
    public string? Ip { get; set; }
    public int Port { get; set; }

    private readonly Channel<ReceiveMessage> _receiveChannel = Channel.CreateUnbounded<ReceiveMessage>(
        new UnboundedChannelOptions
        {
            AllowSynchronousContinuations = false,
            SingleReader = true,
            SingleWriter = true
        });

    readonly Stopwatch _stopwatch = new();

    private readonly TcpClient _client = new();
    private NetworkStream? _networkStream;
    private List<byte> _list = new();
    private CancellationTokenSource? _cts;
    private readonly ILogger<TcpClientNet> _logger;

    public event EventHandler<ClosedArgs>? Closed;
    public event EventHandler<ConnectedArgs>? Connected;
    public void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public Action<ReceiveMessage> ReceiveMessageAction { get; set; }
    public bool IsConnect => _client.Connected;

    public TcpClientNet(ILogger<TcpClientNet> logger)
    {
        _logger = logger;
    }

    public async Task ConnectAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(Ip) || Port <= 0)
            {
                throw new Exception("Ip or Port is null");
            }

            await _client.ConnectAsync(Ip!, Port);
            _networkStream = _client.GetStream();
            Connected?.Invoke(this, new());
            _cts = new CancellationTokenSource();
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Task.Run(ReceiveTask, _cts.Token);
            Task.Run(SignleWrite);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }
        catch (Exception e)
        {
            Closed?.Invoke(this, new());
            _logger.LogError(e, "Connect Field");
        }
    }

    private async Task? SignleWrite()
    {
        await foreach (var args in _receiveChannel.Reader.ReadAllAsync())
        {
            ReceiveMessageAction?.Invoke(args);
            _pool.Return(args);
        }
    }

    private void ReceiveTask()
    {
        try
        {
            Span<byte> buffer = new byte[ReceiveBufferSize];
            while (_cts?.IsCancellationRequested == false)
            {
                if (_stopwatch.IsRunning &&
                    (_stopwatch.ElapsedMilliseconds > AutoBreakTime || !_networkStream!.DataAvailable))
                {
                    var array = _list.ToArray();
                    Console.WriteLine(array.Length);
                    _list.Clear();
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    OnReceive(array);
                }


                if (buffer.Length != ReceiveBufferSize)
                {
                    buffer = new byte[ReceiveBufferSize];
                }
                
                var count = _networkStream!.Read(buffer);
                if (count == 0)
                {
                    CloseAsync();
                    return;
                }

                if (!AutoBreak)
                {
                    OnReceive(buffer.Slice(0, count).ToArray(), false);
                    continue;
                }

                if (!_networkStream.DataAvailable)
                {
                    if (_stopwatch.IsRunning)
                    {
                        _list.AddRange(buffer.Slice(0, count));
                    }
                    else
                    {
                        OnReceive(buffer.Slice(0, count).ToArray());
                    }
                }
                else
                {
                    if (!_stopwatch.IsRunning)
                    {
                        _stopwatch.Start();
                    }

                    _list.AddRange(buffer.Slice(0, count));
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private ReceiveMessagePool _pool = new();

    private async void OnReceive(byte[] buffer, bool autoBreak = true)
    {
        var message = _pool.Rent();
        message.Data = buffer;
        await _receiveChannel.Writer.WriteAsync(message);
    }

    public Task CloseAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        _client.Close();
        Closed?.Invoke(this, new());
        return Task.CompletedTask;
    }

    public async Task SendAsync(byte[] buffer)
    {
        await _networkStream!.WriteAsync(buffer);
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}