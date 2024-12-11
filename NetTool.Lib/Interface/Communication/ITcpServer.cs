using System.Net.Sockets;

namespace NetTool.Lib.Interface;

public interface ITcpServer
{
    /// <summary>
    /// 连接选项
    /// </summary>
    public ITcpServerConnectOption TcpServerConnectOption { get; }

    /// <summary>
    /// 接收选项
    /// </summary>
    public ITcpServerReceiveOption TcpServerReceiveOption { get; }

    /// <summary>
    /// 发送选项
    /// </summary>
    public ITcpServerSendOption TcpServerSendOption { get; }
    
    /// <summary>
    /// 连接事件
    /// </summary>
    event EventHandler<Socket>? ClientConnected;

    /// <summary>
    /// 关闭事件
    /// </summary>
    event EventHandler<Socket>? ClientClosed;

    /// <summary>
    /// 向指定客户端发送数据
    /// </summary>
    public void Write(Socket socket, byte[] buffer, int offset, int count);
    
    /// <summary>
    /// 向指定客户端发送数据
    /// </summary>
    public Task WriteAsync(Socket socket, byte[] buffer, int offset, int count);
}