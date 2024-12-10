using CommunityToolkit.Mvvm.ComponentModel;

namespace NetTool.Models;

public partial class BaseSocketConnectOption : BaseConnectOption
{
    [ObservableProperty] private string? _ip;
    [ObservableProperty] private int _port;
}