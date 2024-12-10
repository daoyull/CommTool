using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

public partial class TcpClientConnectOption : BaseSocketConnectOption, ITcpClientConnectOption
{
    public TcpClientConnectOption()
    {
        Ip = "127.0.0.1";
        Port = 7789;
    }
}