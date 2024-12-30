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

public partial class TcpClientViewModel : AbstractCommViewModel<SocketMessage>, IDisposable
{
    public TcpClientViewModel(TcpClientAdapter tcpClient)
    {
        Client = tcpClient;
        InitCommunication();
    }

    protected sealed override void InitCommunication()
    {
        base.InitCommunication();
        V8Receive.LoadEngine += engine => { engine.AddHostObject("client", new JsTcpClient(this, engine)); };
        V8Send.LoadEngine += engine => { engine.AddHostObject("client", new JsTcpClient(this, engine)); };
    }

    public TcpClientAdapter Client { get; }
    public override ICommunication<SocketMessage> Communication => Client;

    protected override void LogUiReceiveMessage(SocketMessage message)
    {
        if (ReceiveOption.LogStyleShow)
        {
            Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        }

        Ui.Logger.Success($"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogFileReceiveMessage(SocketMessage message)
    {
        FileLog.WriteMessage(Type, $"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        FileLog.WriteMessage(Type, $"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogSendMessage(byte[] bytes)
    {
        var time = DateTime.Now;
        Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] Send");
        Ui.Logger.Primary(bytes.BytesToString(SendOption.IsHex));
    }

    protected override void LogFileSendMessage(byte[] buffer)
    {
        FileLog.WriteMessage(Type, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        FileLog.WriteMessage(Type, $"{buffer.BytesToString(SendOption.IsHex)}");
    }

    protected override object InvokeSendScript(byte[] buffer)
    {
        var array = (ITypedArray<byte>)V8Send.Engine!.Invoke("arrayToUint8Array", buffer);
        var jsMessage = new JsSocketMessage(array, DateTime.Now, Client.Client!.Client,true);
        return V8Send.Engine.Invoke("send", jsMessage);
    }

    protected override object InvokeReceiveScript(SocketMessage message)
    {
        var array = (ITypedArray<byte>)V8Receive.Engine!.Invoke("arrayToUint8Array", message.Data);
        var jsMessage = new JsSocketMessage(array, message.Time, message.Socket,false);
        return V8Receive.Engine.Invoke("receive", jsMessage);
    }


    protected override string Type => "TcpClient";

    public void Dispose()
    {
        Client.Dispose();
    }
}