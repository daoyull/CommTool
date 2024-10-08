using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

public partial class TcpServerOption : ObservableObject, ITcpServerOption
{
    [ObservableProperty] private string? _ip = "127.0.0.1";
    [ObservableProperty] private int _port = 7789;
}

public partial class TcpServerReceiveOption : ObservableObject, ITcpServerReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoBreakFrame;
    [ObservableProperty] private int _autoBreakFrameTime = 20;
    [ObservableProperty] private bool _autoNewLine;
}

public partial class TcpServerSendOption : ObservableObject, ITcpServerSendOption
{
    [ObservableProperty] private bool _defaultWriteUi;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoSend;
    [ObservableProperty] private int _autoSendTime = 50;
}