using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;
using Comm.WPF.Abstracts.Plugins;

namespace Comm.WPF.ViewModels;

public partial class TcpClientViewModel : AbstractCommViewModel<SocketMessage>, IDisposable
{
    public TcpClientViewModel(TcpClientAdapter tcpClient)
    {
        Client = tcpClient;
    }

    public TcpClientAdapter Client { get; }
    public override ICommunication<SocketMessage> Communication => Client;

    protected override void HandleReceiveMessage(SocketMessage message, string strMessage)
    {
        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive]");
        Ui.Logger.Success($"{strMessage}");
    }

    protected override void HandleSendMessage(byte[] bytes, string message)
    {
        var time = DateTime.Now;
        Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
        Ui.Logger.Write($"{message}", "#1E6FFF");
    }

    protected override void OnSendScript(byte[] buffer, string uiMessage)
    {
        // 脚本
        var plugin = (SendScriptPlugin<SocketMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(SendScriptPlugin<SocketMessage>));
        plugin?.InvokeScript(engine =>
        {
            var array = engine.Script.arrayToUint8Array(buffer);
            engine.Script.send(array, DateTime.Now, uiMessage);
        });
    }

    protected override void OnReceiveScript(SocketMessage message, string uiMessage)
    {
        // 脚本
        var plugin = (ReceiveScriptPlugin<SocketMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(ReceiveScriptPlugin<SocketMessage>));
        plugin?.InvokeScript(engine =>
        {
            var array = engine.Script.arrayToUint8Array(message.Data);
            engine.Script.receive(array, message.Time, uiMessage);
        });
    }

    public override string ScriptType => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}