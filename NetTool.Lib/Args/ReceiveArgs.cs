namespace NetTool.Lib.Args;

public class ReceiveArgs : EventArgs
{
    public ReceiveArgs(byte[] buffer)
    {
        Buffer = buffer;
    }

    public byte[] Buffer { get; }
}