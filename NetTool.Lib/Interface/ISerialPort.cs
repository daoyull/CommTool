using System.IO.Ports;

namespace NetTool.Lib.Interface;

public interface ISerialPort
{
    public ISerialOption SerialOption { get;  }

    public void Connect();

    public void Close();

    public List<string> GetPortNames();
}