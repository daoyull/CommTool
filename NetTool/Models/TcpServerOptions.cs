using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;
using NetTool.Module.Options;

namespace NetTool.Models;

public partial class TcpServerOption : ObservableObject, ITcpServerOption
{
    [ObservableProperty] private string? _ip = "127.0.0.1";
    [ObservableProperty] private int _port = 7789;
}

public partial class TcpServerReceiveOption : BaseReceiveOption, ITcpServerReceiveOption
{
    public bool AutoBreakFrame { get; set; }
    public int AutoBreakFrameTime { get; set; }
}

public partial class TcpServerSendOption : BaseSendOption, ITcpServerSendOption
{
}