namespace NetTool.Lib.Messages;

public class ReceiveMessage
{
    internal ReceiveMessage()
    {
    }

    public string? Ip { get; set; }

    public int? Port { get; set; }

    public byte[]? Data { get; set; }

    public DateTime? Time { get; set; }

    public void Reset()
    {
        Ip = null;
        Port = null;
        Data = null;
        Time = null;
    }
}