namespace NetTool.Lib.Interface;

public interface ISocketConnectOption : IConnectOption
{
    public string? Ip { get; set; }

    public int Port { get; set; }
}