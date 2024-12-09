using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

public partial class BaseReceiveOption : ObservableObject, IReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _autoScroll;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _autoNewLine;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private bool _isEnableScriptDebug;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private int _maxFrameSize;
    [ObservableProperty] private int _maxFrameTime;
}