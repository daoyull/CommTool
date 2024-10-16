using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Module.Options;

public partial class BaseSendOption : ObservableObject, ISendOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoSend;
    [ObservableProperty] private int _autoSendTime = 100;
}