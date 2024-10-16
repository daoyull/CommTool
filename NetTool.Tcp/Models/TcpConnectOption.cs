using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.TcpClient.Options;

namespace NetTool.TcpClient.Models;

public partial class TcpConnectOption : ObservableObject, ITcpConnectOption
{
    [ObservableProperty] private string? _ip;
    
    [ObservableProperty] private int _port;
}