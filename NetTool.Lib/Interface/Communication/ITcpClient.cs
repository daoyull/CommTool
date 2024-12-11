namespace NetTool.Lib.Interface;

public interface ITcpClient
{
    /// <summary>
    /// 连接选项
    /// </summary>
    public ITcpClientConnectOption TcpClientConnectOption { get; }
    
    /// <summary>
    /// 接收选项
    /// </summary>
    public ITcpClientReceiveOption TcpClientReceiveOption { get; }
    
    /// <summary>
    /// 发送选项
    /// </summary>
    public ITcpClientSendOption TcpClientSendOption { get; }
    
}