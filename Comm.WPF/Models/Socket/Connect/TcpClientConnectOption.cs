using CommunityToolkit.Mvvm.ComponentModel;
using Comm.Lib.Interface;

namespace Comm.WPF.Models;

public partial class TcpClientConnectOption : BaseSocketConnectOption, ITcpClientConnectOption
{
    public TcpClientConnectOption()
    {
        Ip = "127.0.0.1";
        Port = 7789;
    }
}