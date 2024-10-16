using NetTool.Lib.Interface;

namespace NetTool.TcpClient.Options;

public interface ITcpConnectOption : IConnectOption
{
    public string? Ip { get; set; }
    
    public int Port { get; set; }
}