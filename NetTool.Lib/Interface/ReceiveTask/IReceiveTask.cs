namespace NetTool.Lib.Interface;

public interface IReceiveTask
{
    public int CanReadByte { get; }

    public IReceiveOption ReceiveOption { get; }

    public event EventHandler<byte[]>? FrameReceive;

    public int Read(byte[] buffer, int size);

    public CancellationTokenSource Cts { get; }

    public Action? CloseEvent { get; set; }

    public Task StartTask();
}