using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Args;
using NetTool.Lib.Entity;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class TcpServerAdapter : AbstractCommunication<TcpServerMessage>, ITcpServer
{
    private TcpListener? _listener;

    public TcpServerAdapter(INotify notify, IGlobalOption globalOption, ITcpServerOption tcpServerOption,
        ITcpServerReceiveOption tcpServerReceiveOption, ITcpServerSendOption tcpServerSendOption) : base(notify,
        globalOption)
    {
        TcpServerOption = tcpServerOption;
        TcpServerReceiveOption = tcpServerReceiveOption;
        TcpServerSendOption = tcpServerSendOption;
    }


    public override IReceiveOption ReceiveOption => TcpServerReceiveOption;
    public override ISendOption SendOption => TcpServerSendOption;

    public override void Write(byte[] buffer, int offset, int count)
    {
        var clientItem = Clients.FirstOrDefault(it => it.IsPrimary);
        if (clientItem != null)
        {
            clientItem.Socket.Send(buffer.AsSpan().Slice(offset, count));
        }
    }

    public ITcpServerOption TcpServerOption { get; }
    public ITcpServerReceiveOption TcpServerReceiveOption { get; }
    public ITcpServerSendOption TcpServerSendOption { get; }
    public ObservableCollection<TcpClientItem> Clients { get; } = new();

    private CancellationTokenSource? _connectCts;

    public void Listen()
    {
        _connectCts = new();
        _listener = new TcpListener(IPAddress.Any, TcpServerOption.Port);
        _listener.Start();
        IsConnect = true;
        OnConnected(new ConnectedArgs());
        Task.Run(StartListen, _connectCts.Token);
    }

    private void StartListen()
    {
        while (_connectCts is { IsCancellationRequested: false })
        {
            var socket = _listener.AcceptSocket();
            if (socket.Connected)
            {
                var tcpClientItem = new TcpClientItem(socket);
                tcpClientItem.IsPrimary = true;
                Clients.Add(tcpClientItem);
                Task.Run(() => HandleClientReceive(tcpClientItem), _connectCts.Token);
            }
        }
    }

    private void HandleClientReceive(TcpClientItem item)
    {
        var client = item.Socket;
        var stopwatch = item.StopWatch;
        var list = item.List;
        try
        {
            byte[] buffer = new byte[GlobalOption.BufferSize];
            while (_connectCts is { IsCancellationRequested: false })
            {
                if (stopwatch.IsRunning &&
                    (stopwatch.ElapsedMilliseconds > TcpServerReceiveOption.AutoBreakFrameTime ||
                     client.Available != 0))
                {
                    var array = list.ToArray();
                    Console.WriteLine(array.Length);
                    list.Clear();
                    stopwatch.Reset();
                    stopwatch.Stop();
                    WriteMessage(new((client), array));
                }


                if (buffer.Length != GlobalOption.BufferSize)
                {
                    buffer = new byte[GlobalOption.BufferSize];
                }

                var count = client.Receive(buffer);
                if (count == 0)
                {
                    ClientClose(item);
                    return;
                }

                if (!TcpServerReceiveOption.AutoBreakFrame)
                {
                    WriteMessage(new((client), buffer[..count]));
                    continue;
                }

                if (client.Available == 0)
                {
                    if (stopwatch.IsRunning)
                    {
                        list.AddRange(buffer[..count]);
                    }
                    else
                    {
                        WriteMessage(new((client), buffer[..count]));
                    }
                }
                else
                {
                    if (!stopwatch.IsRunning)
                    {
                        stopwatch.Start();
                    }

                    list.AddRange(buffer[..count]);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ClientClose(TcpClientItem item)
    {
        item.Socket.Dispose();
        Clients.Remove(item);
    }

    private void Close()
    {
        OnClosed(new ClosedArgs());
        IsConnect = false;
        Clients.Clear();
        _connectCts?.Cancel();
        _connectCts?.Dispose();
        _connectCts = null;
        _listener?.Stop();
        _listener?.Dispose();
        _listener = null;
    }
}