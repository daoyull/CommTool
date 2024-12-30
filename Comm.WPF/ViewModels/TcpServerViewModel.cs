using System.Collections.ObjectModel;
using System.Net.Sockets;
using CommunityToolkit.Mvvm.ComponentModel;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.Service.Share;
using Comm.WPF.Abstracts;
using Comm.WPF.Common;
using Comm.WPF.Entity;
using Comm.WPF.Servcice.V8;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.ViewModels;

public partial class TcpServerViewModel : AbstractCommViewModel<SocketMessage>, IDisposable
{
    public TcpServerViewModel(TcpServerAdapter tcpServerAdapter)
    {
        Server = tcpServerAdapter;
        tcpServerAdapter.ClientConnected += HandleClientConnected;
        tcpServerAdapter.ClientClosed += HandleClientClosed;
        tcpServerAdapter.Connected += (_, _) => { Clients.Clear(); };
        tcpServerAdapter.Closed += (_, _) => { Clients.Clear(); };
        InitCommunication();
    }

    protected sealed override void InitCommunication()
    {
        base.InitCommunication();
        V8Receive.LoadEngine += engine => { engine.AddHostObject("server", new JsTcpServer(this, engine)); };
        V8Send.LoadEngine += engine => { engine.AddHostObject("server", new JsTcpServer(this, engine)); };
    }

    private void HandleClientClosed(object? sender, Socket e)
    {
        var item = _clientList.FirstOrDefault(it => it.Socket == e);
        if (item != null)
        {
            _clientList.Remove(item);
        }

        Clients = new(_clientList);
        Ui.Logger.Warning($"{e.ToRemoteIpStr()} 断开连接");
    }


    [ObservableProperty] private ObservableCollection<ClientItem> _clients = new();

    private List<ClientItem> _clientList = new();

    private void HandleClientConnected(object? sender, Socket e)
    {
        _clientList.Add(new ClientItem(e));
        Clients = new(_clientList);
    }


    public TcpServerAdapter Server { get; }

    public override ICommunication<SocketMessage> Communication => Server;


    protected override void LogUiReceiveMessage(SocketMessage message)
    {
        if (ReceiveOption.LogStyleShow)
        {
            Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive <-- {message.RemoteIp}]");
        }

        Ui.Logger.Success($"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogFileReceiveMessage(SocketMessage message)
    {
        FileLog.WriteMessage(Type, $"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive <-- {message.RemoteIp}]");
        FileLog.WriteMessage(Type, $"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogSendMessage(byte[] bytes)
    {
        var clientItems = _clientList.Where(it => it.IsSelected).Select(it => it.ShowName).ToList();
        if (clientItems.Count == 0)
        {
            return;
        }

        var sendStr = $"[{string.Join(',', clientItems)}]";
        Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send --> {sendStr}]");
        Ui.Logger.Success($"{bytes.BytesToString(SendOption.IsHex)}");
    }

    protected override void LogFileSendMessage(byte[] buffer)
    {
        var clientItems = _clientList.Where(it => it.IsSelected).Select(it => it.ShowName).ToList();
        if (clientItems.Count == 0)
        {
            return;
        }

        var sendStr = $"[{string.Join(',', clientItems)}]";
        FileLog.WriteMessage(Type, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send --> {sendStr}]");
        FileLog.WriteMessage(Type, $"{buffer.BytesToString(SendOption.IsHex)}");
    }

    protected override object InvokeSendScript(byte[] buffer)
    {
        var array = (ITypedArray<byte>)V8Send.Engine!.Invoke("arrayToUint8Array", buffer);
        var jsMessage = new JsSocketMessage(array, DateTime.Now, Server.Listener!.Server,true);
        return V8Send.Engine.Invoke("send", jsMessage);
    }

    protected override object InvokeReceiveScript(SocketMessage message)
    {
        var array = (ITypedArray<byte>)V8Receive.Engine!.Invoke("arrayToUint8Array", message.Data);
        var jsMessage = new JsSocketMessage(array, message.Time, message.Socket,false);
        return V8Receive.Engine.Invoke("receive", jsMessage);
    }


    protected override async Task SendBytes(byte[] buffer)
    {
        var list = _clientList.Where(it => it.IsSelected).ToList();
        if (list.Count == 0)
        {
            return;
        }

        foreach (var item in list)
        {
            await Server.WriteAsync(item.Socket, buffer, 0, buffer.Length);
            Ui?.AddSendFrame(1);
            Ui?.AddSendBytes((uint)buffer.Length);
        }
    }

    protected override string Type => "TcpServer";

    public void Dispose()
    {
        Server.Dispose();
    }
}

public partial class ClientItem : ObservableObject
{
    public Socket Socket { get; }

    [ObservableProperty] private bool _isSelected = true;

    [ObservableProperty] private string _showName;

    public ClientItem(Socket socket)
    {
        Socket = socket;
        ShowName = socket.ToRemoteIpStr();
    }
}