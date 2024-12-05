using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;

namespace NetTool.ViewModels;

public partial class TcpClientViewModel : AbstractNetViewModel<TcpClientMessage>, IDisposable
{
    public TcpClientViewModel(TcpClientAdapter tcpClient) 
    {
        Client = tcpClient;
    }


    public TcpClientAdapter Client { get; }
    public override ICommunication<TcpClientMessage> Communication => Client;

    protected override async Task Connect()
    {
        try
        {
            if (!IsConnect)
            {
                await Client.ConnectAsync();
            }
            else
            {
                Client.Close();
            }
        }
        catch (Exception e)
        {
            Client.Notify.Error(e.Message);
        }
    }


    protected override void HandleReceiveMessage(TcpClientMessage message, string strMessage)
    {
        if (Ui == null)
        {
            return;
        }
        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive]");
        Ui.Logger.Success($"{strMessage}");
        if (ReceiveOption.AutoNewLine)
        {
            Ui.Logger.Message(string.Empty,string.Empty);
        }
    }

    protected override void HandleSendMessage(string message)
    {
        if (Ui == null)
        {
            return;
        }
        Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
        Ui.Logger.Message($"{message}", "#1E6FFF");
        if (ReceiveOption.AutoNewLine)
        {
            Ui.Logger.Message(string.Empty,string.Empty);
        }
    }

    public override string ScriptType => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}