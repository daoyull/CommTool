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

    public override string ScriptType => "Udp";

    public override ICommunication<SocketMessage> Communication { get; }

    protected override void HandleReceiveMessage(SocketMessage message, string strMessage)
    {
        Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] [Receive:{message.RemoteIp}]");
        Ui.Logger.Success($"{strMessage}");
    }

    protected override void HandleSendMessage(byte[] bytes, string message)
    {
        Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send:{UdpAdapter.UdpSendOption.SendIp}]");
        Ui.Logger.Primary($"{message}");
    }
}