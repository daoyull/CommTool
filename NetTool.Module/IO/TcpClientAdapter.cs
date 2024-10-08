using System.Diagnostics;
using System.Net.Sockets;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class TcpClientAdapter : AbstractCommunication<TcpClientMessage>, ITcpClient
{
    public TcpClientAdapter(INotify notify, IGlobalOption globalOption, ITcpClientOption clientOption,
        ITcpClientReceiveOption clientReceiveOption, ITcpClientSendOption clientSendOption) : base(notify, globalOption)
    {
        TcpClientOption = clientOption;
        TcpClientReceiveOption = clientReceiveOption;
        TcpClientSendOption = clientSendOption;
    }

    private TcpClient? _client;

    #region 只读属性

    public override IReceiveOption ReceiveOption => TcpClientReceiveOption;
    public override ISendOption SendOption => TcpClientSendOption;
    public ITcpClientOption TcpClientOption { get; }
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    public ITcpClientSendOption TcpClientSendOption { get; }


    private CancellationTokenSource? _rcCts;

    public async Task ConnectAsync()
    {
        try
        {
            if (IsConnect)
            {
                Close();
            }

            if (string.IsNullOrEmpty(TcpClientOption.Ip) || TcpClientOption.Port <= 0)
            {
                throw new Exception("Ip or Port is null");
            }

            _client = new();
            _rcCts = new();
            await _client.ConnectAsync(TcpClientOption.Ip, TcpClientOption.Port);
            _networkStream = _client.GetStream();
            OnConnected(new());
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Task.Run(ReceiveTask, _rcCts.Token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }
        catch (Exception e)
        {
            Close();
            throw;
        }
    }

    readonly Stopwatch _stopwatch = new();

    private NetworkStream? _networkStream;
    private readonly List<byte> _list = new();

    private void ReceiveTask()
    {
        try
        {
            byte[] buffer = new byte[GlobalOption.BufferSize];
            while (_client != null && _rcCts is { IsCancellationRequested: false })
            {
                if (_stopwatch.IsRunning &&
                    (_stopwatch.ElapsedMilliseconds > TcpClientReceiveOption.AutoBreakFrameTime ||
                     !_networkStream!.DataAvailable))
                {
                    var array = _list.ToArray();
                    Console.WriteLine(array.Length);
                    _list.Clear();
                    _stopwatch.Reset();
                    _stopwatch.Stop();
                    WriteMessage(new TcpClientMessage(array));
                }


                if (buffer.Length != GlobalOption.BufferSize)
                {
                    buffer = new byte[GlobalOption.BufferSize];
                }

                var count = _networkStream!.Read(buffer);
                if (count == 0)
                {
                    Close();
                    return;
                }

                if (!TcpClientReceiveOption.AutoBreakFrame)
                {
                    WriteMessage(new TcpClientMessage(buffer[..count]));
                    continue;
                }

                if (!_networkStream.DataAvailable)
                {
                    if (_stopwatch.IsRunning)
                    {
                        _list.AddRange(buffer[..count]);
                    }
                    else
                    {
                        WriteMessage(new TcpClientMessage(buffer[..count]));
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void Close()
    {
        _rcCts?.Cancel();
        _rcCts?.Dispose();
        _rcCts = null;
        if (_client != null)
        {
            _client.Close();
            _client.Dispose();
            _client = null;
        }

        OnClosed(new());
    }

    #endregion

    protected override void Dispose(bool isDispose)
    {
        if (isDispose)
        {
            Close();
        }

        base.Dispose(isDispose);
    }


    public override void Write(byte[] buffer, int offset, int count)
    {
        if (_client != null && _networkStream != null)
        {
            _networkStream.Write(buffer.AsSpan().Slice(offset, count));
        }
    }
}