using System.IO.Ports;

namespace Comm.Lib.Interface;

public interface ISerialConnectOption : IConnectOption
{
    public string? SerialPortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits? StopBits { get; set; }

    public Parity? Parity { get; set; }
}