using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Module.Options;

namespace NetTool.Models;

public partial class TcpConnectOption : ObservableObject, ITcpConnectOption
{
    [ObservableProperty] private string? _ip;

    [ObservableProperty] private int _port;
}