namespace Comm.Lib.Interface;

public interface IUdp
{
    /// <summary>
    /// Udp连接选项
    /// </summary>
    public IUdpConnectOption UdpConnectOption { get; set; }

    /// <summary>
    /// Udp接收选项
    /// </summary>
    public IUdpReceiveOption UdpReceiveOption { get; set; }

    /// <summary>
    /// Udp发送选项
    /// </summary>
    public IUdpSendOption UdpSendOption { get; set; }

    public Task Write(string ip,int port, byte[] buffer);
    
    public Task Write(string address, byte[] buffer);
}