using CommunityToolkit.Mvvm.ComponentModel;

namespace NetTool.Models;

public partial class ReceiveOption : ObservableObject
{
    [ObservableProperty] private bool _hexDisplay;
    [ObservableProperty] private bool _autoNewLine;
    [ObservableProperty] private bool _savaToFile;
    [ObservableProperty] private bool _autoBreak = true;
    [ObservableProperty] private int _autoBreakTime = 20;
}