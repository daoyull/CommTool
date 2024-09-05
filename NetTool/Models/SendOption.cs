using CommunityToolkit.Mvvm.ComponentModel;

namespace NetTool.Models;

public partial class SendOption : ObservableObject
{
    [ObservableProperty] private bool _hexSend;
    [ObservableProperty] private bool _useScript;
    [ObservableProperty] private bool _scheduleSend;
    [ObservableProperty] private int _scheduleSendTime = 200;
}