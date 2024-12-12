namespace Comm.Lib.Interface;

public interface IUdpSendOption : ISendOption
{
    public string? SendIp { get; set; }

    public int SendPort { get; set; }
}