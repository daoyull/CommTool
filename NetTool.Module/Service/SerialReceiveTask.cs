using System.IO.Ports;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public class SerialReceiveTask(SerialPort serialPort, IReceiveOption receiveOption, CancellationTokenSource cts)
    : AbstractReceiveTask(receiveOption, cts)
{
    protected override bool IsBreakConnect => false;
    public override int CanReadByte => serialPort.BytesToRead;

    public override int Read(byte[] buffer, int size)
    {
        return serialPort.Read(buffer, 0, size);
    }
}