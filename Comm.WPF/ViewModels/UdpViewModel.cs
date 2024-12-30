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

public class UdpViewModel : AbstractCommViewModel<UdpMessage>
{
    public UdpAdapter UdpAdapter { get; }

    public UdpViewModel(UdpAdapter udpAdapter)
    {
        Communication = UdpAdapter = udpAdapter;
        InitCommunication();
    }

    protected sealed override void InitCommunication()
    {
        base.InitCommunication();
        V8Receive.LoadEngine += engine => { engine.AddHostObject("udp", new JsUdp(this, engine)); };
        V8Send.LoadEngine += engine => { engine.AddHostObject("udp", new JsUdp(this, engine)); };
    }

    protected override string Type => "Udp";

    public override ICommunication<UdpMessage> Communication { get; }

    protected override void LogUiReceiveMessage(UdpMessage message)
    {
        if (ReceiveOption.LogStyleShow)
        {
            Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive <-- {message.RemoteIp}]");
        }

        Ui.Logger.Success($"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogFileReceiveMessage(UdpMessage message)
    {
        FileLog.WriteMessage(Type, $"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive <-- {message.RemoteIp}]");
        FileLog.WriteMessage(Type, $"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogSendMessage(byte[] bytes)
    {
        Ui.Logger.Info(
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send --> {UdpAdapter.UdpSendOption.SendIp}:{UdpAdapter.UdpConnectOption.Port}]");
        Ui.Logger.Primary($"{bytes.BytesToString(SendOption.IsHex)}");
    }

    protected override void LogFileSendMessage(byte[] buffer)
    {
        FileLog.WriteMessage(Type,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send --> {UdpAdapter.UdpSendOption.SendIp}:{UdpAdapter.UdpConnectOption.Port}]");
        FileLog.WriteMessage(Type, $"{buffer.BytesToString(SendOption.IsHex)}");
    }

    protected override object InvokeSendScript(byte[] buffer)
    {
        var array = (ITypedArray<byte>)V8Send.Engine!.Invoke("arrayToUint8Array", buffer);
        var jsMessage = new JsUdpMessage(array, DateTime.Now, UdpAdapter.UdpSendOption.SendIp!,
            UdpAdapter.UdpSendOption.SendPort);
        return V8Send.Engine.Invoke("send", jsMessage);
    }

    protected override object InvokeReceiveScript(UdpMessage message)
    {
        var array = (ITypedArray<byte>)V8Receive.Engine!.Invoke("arrayToUint8Array", message.Data);
        var jsMessage = new JsUdpMessage(array, message.Time, message.Ip, message.Port);
        return V8Receive.Engine.Invoke("receive", jsMessage);
    }
}