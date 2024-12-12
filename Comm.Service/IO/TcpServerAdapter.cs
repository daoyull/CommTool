using System.Net;
using System.Net.Sockets;
using Comm.Lib.Args;
using Comm.Lib.Interface;
using Comm.Service.Messages;
using Comm.Service.Service;

namespace Comm.Service.IO;

public class TcpServerAdapter : AbstractCommunication<SocketMessage>, ITcpServer
{
    private TcpListener? _listener;

    public TcpServerAdapter(ITcpServerConnectOption tcpServerConnectOption,
        ITcpServerReceiveOption tcpServerReceiveOption, ITcpServerSendOption tcpServerSendOption)
    {
        TcpServerConnectOption = tcpServerConnectOption;
        TcpServerReceiveOption = tcpServerReceiveOption;
        TcpServerSendOption = tcpServerSendOption;
    }

    #region Option

    public override IConnectOption ConnectOption => TcpServerConnectOption;
    public override IReceiveOption ReceiveOption => TcpServerReceiveOption;
    public override ISendOption SendOption => TcpServerSendOption;

    public ITcpServerConnectOption TcpServerConnectOption { get; }
    public ITcpServerReceiveOption TcpServerReceiveOption { get; }
    public ITcpServerSendOption TcpServerSendOption { get; }

    #endregion

    private List<SocketPipeReceiveTask> _clients = new();

    /// <summary>
    /// 向所有客户端发送数据
    /// </summary>
    public override void Write(byte[] buffer, int offset, int count)
    {
        foreach (var client in _clients)
        {
            client.Socket.Send(buffer.AsSpan().Slice(offset, count));
        }
    }

    public override void Connect()
    {
        try
        {
            if (IsConnect)
            {
                Close();
            }

            _listener = new TcpListener(IPAddress.Any, TcpServerConnectOption.Port);
            _listener.Start();
            Cts = new();
            OnConnected(new ConnectedArgs());
            Task.Run(StartListen, Cts.Token);
        }
        catch (Exception e)
        {
            Close();
            Ui.Logger.Warning($"打开TCP Server失败: {e.Message}");
        }
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

        while (Cts is { IsCancellationRequested: false })
        {
            try
            {
                var socket = _listener.AcceptSocket();
                if (socket.Connected)
                {
                    ClientConnected?.Invoke(this, socket);
                    var pipeHandle = new SocketPipeReceiveTask(this, socket, new CancellationTokenSource());
                    _clients.Add(pipeHandle);
                    Task.Run(pipeHandle.StartHandle, pipeHandle.Cts.Token);
                    pipeHandle.CloseEvent += (sender, clientSocket) =>
                    {
                        if (sender is SocketPipeReceiveTask closeSocketHandle)
                        {
                            _clients.Remove(closeSocketHandle);
                            closeSocketHandle.Cts.Cancel();
                            closeSocketHandle.Cts.Dispose();
                            ClientClosed?.Invoke(this, clientSocket);
                        }
                    };
                }
            }
            catch (SocketException)
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
        Cts?.Cancel();
        Cts?.Dispose();
        Cts = null;

        OnClosed(new ClosedArgs());

        _clients.Clear();
        _listener?.Stop();
        _listener?.Dispose();
        _listener = null;
    }
}