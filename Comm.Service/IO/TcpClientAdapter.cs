using System.Net.Sockets;
using Comm.Lib.Interface;
using Comm.Service.Messages;
using Comm.Service.Service;

namespace Comm.Service.IO;

public class TcpClientAdapter : AbstractCommunication<SocketMessage>, ITcpClient
{
    public TcpClientAdapter(ITcpClientConnectOption clientConnectOption,
        ITcpClientReceiveOption clientReceiveOption, ITcpClientSendOption clientSendOption)
    {
        TcpClientConnectOption = clientConnectOption;
        TcpClientReceiveOption = clientReceiveOption;
        TcpClientSendOption = clientSendOption;
    }

    public TcpClient? Client;
    private SocketPipeReceiveTask? _pipeHandle;

    #region Option

    public override IConnectOption ConnectOption => TcpClientConnectOption;
    public override IReceiveOption ReceiveOption => TcpClientReceiveOption;
    public override ISendOption SendOption => TcpClientSendOption;
    public ITcpClientConnectOption TcpClientConnectOption { get; }
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    public ITcpClientSendOption TcpClientSendOption { get; }

    #endregion

    public override void Connect()
    {
        try
        {
            if (IsConnect)
            {
                Close();
            }

            if (string.IsNullOrEmpty(TcpClientConnectOption.Ip) || TcpClientConnectOption.Port <= 0)
            {
                throw new Exception("Ip or Port is null");
            }

            Client = new();
            Client.Connect(TcpClientConnectOption.Ip, TcpClientConnectOption.Port);
            Cts = new();
            OnConnected(new());

            _pipeHandle = new SocketPipeReceiveTask(this, Client!.Client, Cts);
            _pipeHandle.CloseEvent += OnPipeHandleOnCloseEvent;
            Task.Run(_pipeHandle.StartHandle, Cts.Token);
        }
        catch (Exception e)
        {
            Close();
            Ui.Logger.Warning($"连接失败: {e.Message}");
        }
    }

    void OnPipeHandleOnCloseEvent(object? sender, Socket socket)
    {
        Ui.Logger.Warning("连接已断开");
        Close();
        Cts?.Dispose();
    }


    public override void Close()
    {
        if (_pipeHandle != null)
        {
            _pipeHandle.CloseEvent -= OnPipeHandleOnCloseEvent;
        }

        Cts?.Cancel();
        Cts?.Dispose();
        Cts = null;
        if (Client != null)
        {
            Client.Close();
            Client.Dispose();
            Client = null;
        }

        OnClosed(new());
    }

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
        if (IsConnect && Client != null)
        {
            Client.GetStream().Write(buffer.AsSpan().Slice(offset, count));
        }
    }
}