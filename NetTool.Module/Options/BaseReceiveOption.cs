using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Module.Options;

public partial class BaseReceiveOption : ObservableObject, IReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _autoScroll;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _autoNewLine;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
}