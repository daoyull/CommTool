using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public class TcpClientService : ITcpClientService
{
    public int ReceiveBufferSize { get; set; } = 256;

    public long AutoBreakTime { get; set; } = 20;
    public bool AutoBreak { get; set; } = true;
    public string? Ip { get; set; }
    public int Port { get; set; }


    readonly Stopwatch _stopwatch = new();

    private readonly TcpClient _client = new();
    private NetworkStream? _networkStream;
    private List<byte> _list = new();
    private CancellationTokenSource? _cts;
    private readonly ILogger<TcpClientService> _logger;

    public event EventHandler<ReceiveArgs>? Received;
    public event EventHandler<ClosedArgs>? Closed;
    public event EventHandler<ConnectedArgs>? Connected;
    public bool IsConnect => _client.Connected;

    public TcpClientService(ILogger<TcpClientService> logger)
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
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }
        catch (Exception e)
        {
            Closed?.Invoke(this, new());
            _logger.LogError(e, "Connect Field");
        }
    }

    private void ReceiveTask()
    {
        Span<byte> buffer = new byte[ReceiveBufferSize];
        while (_cts?.IsCancellationRequested == false)
        {
            if (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds > AutoBreakTime)
            {
                var array = _list.ToArray();
                _stopwatch.Reset();
                _stopwatch.Stop();
                OnReceive(array);
            }

            
            if (buffer.Length != ReceiveBufferSize)
            {
                buffer = new byte[ReceiveBufferSize];
            }

            var count = _networkStream!.Read(buffer);
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

    private void OnReceive(byte[] buffer, bool autoBreak = true)
    {
        Received?.Invoke(this, new ReceiveArgs(buffer));
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