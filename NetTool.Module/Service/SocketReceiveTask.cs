using System.Diagnostics;
using System.Net.Sockets;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public class SocketReceiveTask(Socket socket, IReceiveOption receiveOption, CancellationTokenSource cts)
    : AbstractReceiveTask(receiveOption, cts)
{
    protected override bool IsBreakConnect => socket.Poll(0, SelectMode.SelectWrite);
    public override int CanReadByte => socket.Available;

    public override int Read(byte[] buffer, int size)
    {
        return socket.Receive(buffer, size, SocketFlags.None);
    }
}