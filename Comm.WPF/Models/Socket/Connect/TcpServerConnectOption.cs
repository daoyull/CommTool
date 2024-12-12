using Comm.Lib.Interface;

namespace Comm.WPF.Models;

public class TcpServerConnectOption : BaseSocketConnectOption, ITcpServerConnectOption
{
    public TcpServerConnectOption()
    {
        Ip = "127.0.0.1";
        Port = 7789;
    }
}