using System.IO.Ports;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;
using NetTool.Module.Service;

namespace NetTool.Module.IO;

public class SerialPortAdapter : AbstractCommunication<SerialPortMessage>, ISerialPort
{
    public SerialPortAdapter(
        INotify notify,
        IGlobalOption globalOption,
        ISerialConnectOption serialConnectOption,
        ISerialReceiveOption receiveOption,
        ISerialSendOption sendOption) : base(notify, globalOption)
    {
        SerialConnectOption = serialConnectOption;
        SerialReceiveOption = receiveOption;
        SerialSendOption = sendOption;
    }

    private SerialPort? _serialPort;


    public ISerialConnectOption SerialConnectOption { get; }
    public ISerialReceiveOption SerialReceiveOption { get; }
    public ISerialSendOption SerialSendOption { get; }

    public void Connect()
    {
        try
        {
            if (IsConnect)
            {
                return;
            }

            _serialPort = new SerialPort();
            _serialPort.PortName = SerialConnectOption.SerialPortName;
            _serialPort.BaudRate = SerialConnectOption.BaudRate;
            _serialPort.Parity = SerialConnectOption.Parity!.Value;
            _serialPort.DataBits = SerialConnectOption.DataBits;
            _serialPort.StopBits = SerialConnectOption.StopBits!.Value;
            _serialPort.ReadBufferSize = 1024 * 1024;
            _serialPort.Open();
            OnConnected(new ConnectedArgs());

            ReceiveTask = new SerialReceiveTask(_serialPort, SerialReceiveOption, Cts!);
            ReceiveTask.FrameReceive += HandleFrameReceive;
            Task.Run(() => ReceiveTask.StartTask(), Cts!.Token);
        }
        catch (Exception e)
        {
            Close();
            throw;
        }
    }

    private void HandleFrameReceive(object? sender, byte[] e)
    {
        Console.WriteLine($"串口接收到{e.Length}字节数据");
        WriteMessage(new SerialPortMessage(e));
    }

    public List<string> GetPortNames() => SerialPort.GetPortNames().ToList();

    public override void Close()
    {
        if (ReceiveTask != null)
        {
            ReceiveTask.FrameReceive -= HandleFrameReceive;
            ReceiveTask = null;
        }

        if (_serialPort != null)
        {
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