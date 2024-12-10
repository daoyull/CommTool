using System.Diagnostics;
using System.Net.Sockets;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;
using NetTool.Module.Service;

namespace NetTool.Module.IO;

public class TcpClientAdapter : AbstractCommunication<TcpClientMessage>, ITcpClient
{
    public TcpClientAdapter(INotify notify, IGlobalOption globalOption, ITcpClientConnectOption clientConnectOption,
        ITcpClientReceiveOption clientReceiveOption, ITcpClientSendOption clientSendOption) : base(notify, globalOption)
    {
        TcpClientConnectOption = clientConnectOption;
        TcpClientReceiveOption = clientReceiveOption;
        TcpClientSendOption = clientSendOption;
    }

    private TcpClient? _client;

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

            _client = new(); _client.Connect(TcpClientConnectOption.Ip, TcpClientConnectOption.Port);
            OnConnected(new());
            
            ReceiveTask = new SocketReceiveTask(_client.Client, TcpClientReceiveOption, Cts!);
            ReceiveTask.FrameReceive += HandleFrameReceive;
            Task.Run(() => ReceiveTask.StartTask(), Cts!.Token);
        }
        catch (Exception e)
        {
            Close();
        }
    }

    private void HandleFrameReceive(object? sender, byte[] e)
    {
        Console.WriteLine($"Tcp Client接收到{e.Length}字节数据");
        WriteMessage(new (e));
    }

    public override void Close()
    {
       
        if (_client != null)
        {
            _client.Close();
            _client.Dispose();
            _client = null;
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
        if (_client != null && IsConnect)
        {
            _client.GetStream().Write(buffer.AsSpan().Slice(offset, count));
        }
    }
}