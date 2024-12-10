using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;

namespace NetTool.ViewModels;

public partial class TcpServerViewModel : AbstractNetViewModel<TcpServerMessage>, IDisposable
{
    public TcpServerViewModel(TcpServerAdapter tcpServerAdapter)
    {
        Server = tcpServerAdapter;
        tcpServerAdapter.ClientConnected += HandleClientConnected;
        tcpServerAdapter.ClientClosed += HandleClientClosed;
    }

    private void HandleClientClosed(object? sender, Socket e)
    {
        var item = _clientList.FirstOrDefault(it => it.Socket == e);
        if (item != null)
        {
            _clientList.Remove(item);
        }

        Clients = new(_clientList);
    }


    [ObservableProperty] private ObservableCollection<ClientItem> _clients;

    private List<ClientItem> _clientList = new();

    private void HandleClientConnected(object? sender, Socket e)
    {
        _clientList.Add(new ClientItem(e));
        Clients = new(_clientList);
    }


    public TcpServerAdapter Server { get; }

    public override ICommunication<TcpServerMessage> Communication => Server;
    

    protected override void HandleReceiveMessage(TcpServerMessage message, string strMessage)
    {
        if (Ui == null)
        {
            return;
        }

        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [{message.RemoteIp}] [Receive]");
        Ui.Logger.Success($"{strMessage}");
        if (ReceiveOption.AutoNewLine)
        {
            Ui.Logger.Write(string.Empty, string.Empty);
        }
    }

    protected override void HandleSendMessage(byte[] bytes,string message)
    {
        if (Ui == null)
        {
            return;
        }

        foreach (var clientItem in _clientList.Where(it => it.IsSelected))
        {
            Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{clientItem.Socket.RemoteEndPoint}] [Send]");
            Ui.Logger.Write($"{message}", "#1E6FFF");
            if (ReceiveOption.AutoNewLine)
            {
                Ui.Logger.Write(string.Empty, string.Empty);
            }
        }
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

    public override string ScriptType => "TcpServer";

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
        ShowName = socket.RemoteEndPoint?.ToString() ?? "Unknown";
    }
}