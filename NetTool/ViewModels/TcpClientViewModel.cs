using NetTool.Abstracts;
using NetTool.Abstracts.Plugins;
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
    
    
    protected override void HandleReceiveMessage(TcpClientMessage message, string strMessage)
    {
        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive]");
        Ui.Logger.Success($"{strMessage}");
       
        // 脚本
        var plugin = (ReceiveScriptPlugin<TcpClientMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(ReceiveScriptPlugin<TcpClientMessage>));
        plugin?.InvokeScript(engine => { engine.Script.receive(message.Data, message.Time, strMessage); });
    }

    protected override void HandleSendMessage(byte[] bytes,string message)
    {
        var time = DateTime.Now;
        Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
        Ui.Logger.Write($"{message}", "#1E6FFF");
        
        // 脚本
        var plugin = (SendScriptPlugin<TcpClientMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(SendScriptPlugin<TcpClientMessage>));
        plugin?.InvokeScript(engine => { engine.Script.send(bytes, time, message); });
    }

    public override string ScriptType => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}