using System.IO.Ports;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class SerialPortAdapter : AbstractCommunication<SerialPortMessage>, ISerialPort
{
    public SerialPortAdapter(INotify notify) : base(notify)
    {
    }

    private SerialPort? _serialPort;

    public string? PortName { get; set; }
    public int BaudRate { get; set; }
    public int DataBits { get; set; }
    public StopBits StopBits { get; set; }
    public Parity Parity { get; set; }

    public void Connect()
    {
        if (IsConnect)
        {
            return;
        }

        _serialPort = new SerialPort();
        _serialPort.PortName = PortName;
        _serialPort.BaudRate = BaudRate;
        _serialPort.Parity = Parity;
        _serialPort.DataBits = DataBits;
        _serialPort.StopBits = StopBits;
        _serialPort.DataReceived += OnDataReceived;
        _serialPort.Open();
        IsConnect = true;
    }

    public List<string> GetPortNames() => SerialPort.GetPortNames().ToList();

    private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is not SerialPort serialPort)
        {
            return;
        }

        int length = serialPort.BytesToRead;
        if (length == 0)
        {
            return;
        }
        byte[] buffer = new byte[length];
        serialPort.Read(buffer, 0, length);
        WriteMessage(new SerialPortMessage(buffer));
    }

    public void Close()
    {
        if (_serialPort != null)
        {
            _serialPort.DataReceived -= OnDataReceived;
            _serialPort?.Close();
            _serialPort?.Dispose();
            _serialPort = null;
        }

        IsConnect = false;
    }

    protected override void Dispose(bool isDispose)
    {
        if (isDispose)
        {
            Close();
            // 未消费的消息清理
        }

        base.Dispose(isDispose);
    }


    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!IsConnect || _serialPort == null)
        {
            Notify.Warning("串口未连接");
            return;
        }

        _serialPort.Write(buffer, offset, count);
    }
}