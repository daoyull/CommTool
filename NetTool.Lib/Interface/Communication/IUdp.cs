namespace NetTool.Lib.Interface;

public interface IUdp
{
    public IUdpConnectOption UdpConnectOption { get; set; }

    public IUdpReceiveOption UdpReceiveOption { get; set; }

    public IUdpSendOption UdpSendOption { get; set; }
}