﻿using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;

namespace Comm.WPF.ViewModels;

public partial class TcpClientViewModel : AbstractCommViewModel<SocketMessage>, IDisposable
{
    public TcpClientViewModel(TcpClientAdapter tcpClient)
    {
        Client = tcpClient;
    }

    public TcpClientAdapter Client { get; }
    public override ICommunication<SocketMessage> Communication => Client;

    protected override void LogReceiveMessage(SocketMessage message, string strMessage)
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
        
    }

    protected override object InvokeReceiveScript(SocketMessage message)
    {
        throw new NotImplementedException();
    }


    protected override string ScriptType => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}