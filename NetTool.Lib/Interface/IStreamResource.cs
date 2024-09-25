namespace NetTool.Lib.Interface;

public interface IStreamResource : IDisposable
{
    public int ReceiveBufferSize { get; set; }
    
    void Write(byte[] buffer, int offset, int count);

    int Read(byte[] buffer, int offset, int count);
}