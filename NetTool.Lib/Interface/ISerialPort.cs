using System.IO.Ports;

namespace NetTool.Lib.Interface;

public interface ISerialPort
{
    public string? PortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits StopBits { get; set; }

    public Parity Parity { get; set; }

    public void Connect();

    public void Close();

    public List<string> GetPortNames();
}