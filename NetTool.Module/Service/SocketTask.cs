using System.Net.Sockets;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public class SocketTask : IReceiveTask
{
    private Socket _socket;

    public SocketTask(Socket socket, IReceiveOption receiveOption, CancellationTokenSource cts)
    {
        _socket = socket;
        ReceiveOption = receiveOption;
        Cts = cts;
    }

    public int CanReadByte { get; }
    public IReceiveOption ReceiveOption { get; }
    public event EventHandler<byte[]>? FrameReceive;
    public int Read(byte[] buffer, int size)
    {
        throw new NotImplementedException();
    }

    public CancellationTokenSource Cts { get; }
    public Action? CloseEvent { get; set; }
    public async Task StartTask()
    {
        byte[] buffer = new byte[1024 * 1024];
        while (!Cts.IsCancellationRequested)
        {
            var size = await _socket.ReceiveAsync(buffer);
            
        }
    }
}