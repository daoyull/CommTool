using System.Diagnostics;
using System.IO.Ports;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class SerialPortAdapter : AbstractCommunication<SerialPortMessage>, ISerialPort
{
    public SerialPortAdapter(
        INotify notify,
        IGlobalOption globalOption,
        ISerialOption serialOption,
        ISerialReceiveOption receiveOption,
        ISerialSendOption sendOption) : base(notify, globalOption)
    {
        SerialOption = serialOption;
        SerialReceiveOption = receiveOption;
        SerialSendOption = sendOption;
    }

    private SerialPort? _serialPort;


    public ISerialOption SerialOption { get; }
    public ISerialReceiveOption SerialReceiveOption { get; }
    public ISerialSendOption SerialSendOption { get; }

    public void Connect()
    {
        if (IsConnect)
        {
            return;
        }

        _serialPort = new SerialPort();
        _serialPort.PortName = SerialOption.SerialPortName;
        _serialPort.BaudRate = SerialOption.BaudRate;
        _serialPort.Parity = SerialOption.Parity!.Value;
        _serialPort.DataBits = SerialOption.DataBits;
        _serialPort.StopBits = SerialOption.StopBits!.Value;
        _serialPort.DataReceived += OnDataReceived;
        _serialPort.Open();
        OnConnected(new ConnectedArgs());
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
            OnClosed(new ClosedArgs());
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


    public override IReceiveOption ReceiveOption => SerialReceiveOption;
    public override ISendOption SendOption => SerialSendOption;


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