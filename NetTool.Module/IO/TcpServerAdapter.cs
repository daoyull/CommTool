using System.Net;
using System.Net.Sockets;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class TcpServerAdapter : AbstractCommunication<SocketMessage>, ITcpServer
{
    private TcpListener? _listener;

    public TcpServerAdapter(INotify notify, IGlobalOption globalOption, ITcpServerConnectOption tcpServerConnectOption,
        ITcpServerReceiveOption tcpServerReceiveOption, ITcpServerSendOption tcpServerSendOption) : base(notify,
        globalOption)
    {
        TcpServerConnectOption = tcpServerConnectOption;
        TcpServerReceiveOption = tcpServerReceiveOption;
        TcpServerSendOption = tcpServerSendOption;
    }


    public override IConnectOption ConnectOption => TcpServerConnectOption;
    public override IReceiveOption ReceiveOption => TcpServerReceiveOption;
    public override ISendOption SendOption => TcpServerSendOption;

    private List<Socket> _clients = new();


    public override void Write(byte[] buffer, int offset, int count)
    {
        foreach (var client in _clients)
        {
            client.Send(buffer.AsSpan().Slice(offset, count));
        }
    }

    public ITcpServerConnectOption TcpServerConnectOption { get; }
    public ITcpServerReceiveOption TcpServerReceiveOption { get; }
    public ITcpServerSendOption TcpServerSendOption { get; }

    private CancellationTokenSource? _connectCts;

    public override void Connect()
    {
        _connectCts = new();
        _listener = new TcpListener(IPAddress.Any, TcpServerConnectOption.Port);
        _listener.Start();
        IsConnect = true;
        OnConnected(new ConnectedArgs());
        Task.Run(StartListen, _connectCts.Token);
    }

    public event EventHandler<Socket>? ClientConnected;
    public event EventHandler<Socket>? ClientClosed;

    public void Write(Socket socket, byte[] buffer, int offset, int count)
    {
        socket.Send(buffer.AsSpan().Slice(offset, count));
    }

    public Task WriteAsync(Socket socket, byte[] buffer, int offset, int count)
    {
        return Task.Factory.StartNew(() => Write(socket, buffer, offset, count));
    }

    private void StartListen()
    {
        if (_listener == null)
        {
            return;
        }

        while (_connectCts is { IsCancellationRequested: false })
        {
            try
            {
                var socket = _listener.AcceptSocket();
                if (socket.Connected)
                {
                    _clients.Add(socket);
                    // var tcpClientItem = new TcpClientItem();
                    // tcpClientItem.ReceiveSocket = new ReceiveSocket(socket,
                    //     bytes => WriteMessage(new(socket, bytes)),
                    //     () =>
                    //     {
                    //         ClientClosed?.Invoke(this, socket);
                    //         tcpClientItem.Cts.Cancel();
                    //         tcpClientItem.Cts.Dispose();
                    //         socket.Dispose();
                    //         _clients.Remove(socket);
                    //     }, TcpServerReceiveOption, GlobalOption, tcpClientItem.Cts);
                    // ClientConnected?.Invoke(this, socket);
                    // Task.Run(() => tcpClientItem.ReceiveSocket.ReceiveTask(), _connectCts.Token);
                }
            }
            catch (SocketException e)
            {
            }
        }
    }

    protected override void Dispose(bool isDispose)
    {
        base.Dispose(isDispose);
        if (isDispose)
        {
            Close();
        }
    }

    public override void Close()
    {
        OnClosed(new ClosedArgs());
        IsConnect = false;
        _connectCts?.Cancel();
        _connectCts?.Dispose();
        _connectCts = null;
        _clients.Clear();
        _listener?.Stop();
        _listener?.Dispose();
        _listener = null;
    }
}