using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

public partial class BaseReceiveOption : ObservableObject, IReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile = false;
    [ObservableProperty] private bool _autoScroll = true;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _autoNewLine = true;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private bool _isEnableScriptDebug;
    [ObservableProperty] private bool _logStyleShow;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private int _maxFrameSize = 255;
    [ObservableProperty] private int _maxFrameTime = 20;
    [ObservableProperty] private bool _isMaxFrameTime = true;
}