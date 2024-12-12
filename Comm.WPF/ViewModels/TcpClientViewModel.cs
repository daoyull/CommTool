﻿using Comm.Lib.Interface;
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

        // 脚本
        var plugin = (ReceiveScriptPlugin<SocketMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(ReceiveScriptPlugin<SocketMessage>));
        plugin?.InvokeScript(engine => { engine.Script.receive(message.Data, message.Time, strMessage); });
    }

    protected override void HandleSendMessage(byte[] bytes, string message)
    {
        var time = DateTime.Now;
        Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
        Ui.Logger.Write($"{message}", "#1E6FFF");

        // 脚本
        var plugin = (SendScriptPlugin<SocketMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(SendScriptPlugin<SocketMessage>));
        plugin?.InvokeScript(engine => { engine.Script.send(bytes, time, message); });
    }

    public override string ScriptType => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}