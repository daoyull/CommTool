using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;

namespace Comm.WPF.ViewModels;

public class UdpViewModel : AbstractCommViewModel<SocketMessage>
{
    public UdpAdapter UdpAdapter { get; }

    public UdpViewModel(UdpAdapter udpAdapter)
    {
        Communication = UdpAdapter = udpAdapter;
    }

    protected override string Type => "Udp";

    public override ICommunication<SocketMessage> Communication { get; }

    protected override void LogUiReceiveMessage(SocketMessage message)
    {
        // Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive:{message.RemoteIp}]");
        // Ui.Logger.Success($"{strMessage}");
    }

    protected override void LogFileReceiveMessage(SocketMessage message)
    {
        throw new NotImplementedException();
    }

    protected override void LogSendMessage(byte[] bytes)
    {
        // Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send:{UdpAdapter.UdpSendOption.SendIp}]");
        // Ui.Logger.Primary($"{message}");
    }

    protected override void LogFileSendMessage(byte[] buffer)
    {
        throw new NotImplementedException();
    }

    protected override object InvokeSendScript(byte[] buffer)
    {
        return null;
    }

    protected override object InvokeReceiveScript(SocketMessage message)
    {
        throw new NotImplementedException();
    }
}