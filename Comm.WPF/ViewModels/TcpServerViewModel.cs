using System.Collections.ObjectModel;
using System.Net.Sockets;
using CommunityToolkit.Mvvm.ComponentModel;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.Service.Share;
using Comm.WPF.Abstracts;

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


    protected override void LogReceiveMessage(SocketMessage message, string strMessage)
    {
        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive:{message.RemoteIp}]");
        Ui.Logger.Success($"{strMessage}");
    }

    protected override void HandleSendMessage(byte[] bytes, string message)
    {
        var clientItems = _clientList.Where(it => it.IsSelected).Select(it => it.ShowName).ToList();
        if (clientItems.Count == 0)
        {
            return;
        }

        var sendStr = $"[{string.Join(',', clientItems)}]";
        Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send] {sendStr}");
        Ui.Logger.Write($"{message}", "#1E6FFF");
    }

    protected override void OnSendScript(byte[] buffer, string uiMessage)
    {
       
    }

    protected override object InvokeReceiveScript(SocketMessage message)
    {
        throw new NotImplementedException();
    }


    protected override async Task<bool> HandleSendBytes(byte[] buffer)
    {
        var list = _clientList.Where(it => it.IsSelected).ToList();
        if (list.Count == 0)
        {
            return false;
        }

        foreach (var item in list)
        {
            await Server.WriteAsync(item.Socket, buffer, 0, buffer.Length);
            Ui?.AddSendFrame(1);
            Ui?.AddSendBytes((uint)buffer.Length);
        }

        return true;
    }

    protected override string ScriptType => "TcpServer";

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