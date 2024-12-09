using NetTool.Lib.Interface;

namespace NetTool.Module.Options;

public interface ITcpConnectOption : IConnectOption
{
    public string? Ip { get; set; }
    
    public int Port { get; set; }
}