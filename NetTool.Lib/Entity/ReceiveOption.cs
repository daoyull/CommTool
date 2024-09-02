using System.Net.Sockets;

namespace NetTool.Lib.Entity;

public class ReceiveOption
{
    public static ReceiveOption Default => new ReceiveOption();

    private TcpClient _tcpClient;
}