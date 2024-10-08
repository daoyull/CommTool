namespace NetTool.Lib.Interface;

public interface IMessage
{
    public DateTime Time { get; }

    public byte[] Data { get; }
}