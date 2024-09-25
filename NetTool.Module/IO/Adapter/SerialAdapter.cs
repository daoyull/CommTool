using System.IO.Ports;
using NetTool.Lib.Interface;

namespace NetTool.Module.IO;

public class SerialAdapter : IStreamResource
{
    public int ReceiveBufferSize { get; set; }

    private SerialPort? _serialPort;

    public SerialAdapter(SerialPort serialPort)
    {
        _serialPort = serialPort;
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        return _serialPort!.Read(buffer, offset, count);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        _serialPort!.Write(buffer, offset, count);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _serialPort?.Dispose();
            _serialPort = null;
        }
    }
}