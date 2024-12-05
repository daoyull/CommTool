namespace NetTool.Lib.Interface;

public interface IReceiveTask
{
    public int CanReadByte { get; }

    public int ReadByte(byte[] buffer);

    public bool TimeBreak { get; }

    public int AutoBreakTime { get; }

    public int MaxByteSize { get; }
}