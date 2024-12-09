using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;
using NetTool.Module.Options;

namespace NetTool.Models;

partial class TcpClientOption : ObservableObject, ITcpClientOption
{
    [ObservableProperty] private string? _ip = "127.0.0.1";
    [ObservableProperty] private int _port = 7789;
}

partial class TcpClientReceiveOption : BaseReceiveOption, ITcpClientReceiveOption
{
    public bool AutoBreakFrame { get; set; }
    public int AutoBreakFrameTime { get; set; }
}

partial class TcpClientSendOption : BaseSendOption, ITcpClientSendOption
{
   
}