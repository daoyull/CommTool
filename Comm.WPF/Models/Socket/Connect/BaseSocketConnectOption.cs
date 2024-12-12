using CommunityToolkit.Mvvm.ComponentModel;

namespace Comm.WPF.Models;

public partial class BaseSocketConnectOption : BaseConnectOption
{
    [ObservableProperty] private string? _ip;
    [ObservableProperty] private int _port;
}