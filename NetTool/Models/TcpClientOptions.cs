using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

partial class TcpClientOption : ObservableObject, ITcpClientOption
{
    [ObservableProperty] private string? _ip = "127.0.0.1";
    [ObservableProperty] private int _port = 7789;
}

partial class TcpClienReceiveOption : ObservableObject, ITcpClientReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoBreakFrame;
    [ObservableProperty] private int _autoBreakFrameTime = 20;
}

partial class TcpClienSendOption : ObservableObject, ITcpClientSendOption
{
    [ObservableProperty] private bool _defaultWriteUi;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoSend;
    [ObservableProperty] private int _autoSendTime;
}