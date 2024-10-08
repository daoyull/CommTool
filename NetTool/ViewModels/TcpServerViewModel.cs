using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;

namespace NetTool.ViewModels;

public partial class TcpServerViewModel : AbstractNetViewModel<TcpServerMessage>, IDisposable
{
    public TcpServerViewModel(INotify notify, IGlobalOption globalOption, TcpServerAdapter tcpServerAdapter) : base(
        notify, globalOption)
    {
        Server = tcpServerAdapter;
        InitCommunication();
    }
    
    
    public TcpServerAdapter Server { get; }

    public override ICommunication<TcpServerMessage> Communication => Server;

    protected override Task Connect()
    {
        Server.Listen();
        return Task.CompletedTask;
    }

    protected override void HandleReceiveMessage(TcpServerMessage message, string strMessage)
    {
        if (Ui == null)
        {
            return;
        }

        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [{message.Client.RemoteEndPoint}] [Receive]");
        Ui.Logger.Success($"{strMessage}");
        if (ReceiveOption.AutoNewLine)
        {
            Ui.Logger.Message(string.Empty, string.Empty);
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
            Ui.Logger.Message(string.Empty, string.Empty);
        }
    }

    public void Dispose()
    {
        Server.Dispose();
    }
}