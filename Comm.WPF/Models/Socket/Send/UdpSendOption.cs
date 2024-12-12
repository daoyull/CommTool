using CommunityToolkit.Mvvm.ComponentModel;
using Comm.Lib.Interface;

namespace Comm.WPF.Models;

public partial class UdpSendOption : BaseSendOption, IUdpSendOption
{
    public UdpSendOption()
    {
        SendIp = "127.0.0.1";
        SendPort = 7789;
    }
    [ObservableProperty] private string? _sendIp;
    [ObservableProperty] private int _sendPort;
}