using System.IO.Ports;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;
using NetTool.Module.Service;

namespace NetTool.Module.IO;

public class SerialPortAdapter : AbstractCommunication<SerialMessage>, ISerialPort
{
    public SerialPortAdapter(
        ISerialConnectOption serialConnectOption,
        ISerialReceiveOption receiveOption,
        ISerialSendOption sendOption) 
    {
        SerialConnectOption = serialConnectOption;
        SerialReceiveOption = receiveOption;
        SerialSendOption = sendOption;
    }
    
    private SerialPort? _serialPort;

    #region Option

    public override IConnectOption ConnectOption => SerialConnectOption;
    public override IReceiveOption ReceiveOption => SerialReceiveOption;
    public override ISendOption SendOption => SerialSendOption;
    public ISerialConnectOption SerialConnectOption { get; }
    public ISerialReceiveOption SerialReceiveOption { get; }
    public ISerialSendOption SerialSendOption { get; }

    #endregion

    public override void Connect()
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
            _serialPort.Open();
            Cts = new();
            OnConnected(new ConnectedArgs());

            var serialPipeHandle = new SerialPipeHandle(_serialPort, this, Cts);
            Task.Run(() => serialPipeHandle.StartHandle(),Cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Close();
        }
    }
    

    public List<string> GetPortNames() => SerialPort.GetPortNames().ToList();

    public override void Close()
    {
        Cts?.Cancel();
        Cts?.Dispose();
        Cts = null;
        
        if (_serialPort != null)
        {
            _serialPort?.Close();
            _serialPort?.Dispose();
            _serialPort = null;
        }
        OnClosed(new ClosedArgs());
    }

    protected override void Dispose(bool isDispose)
    {
        if (isDispose)
        {
            Close();
        }

        base.Dispose(isDispose);
    }

    
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!IsConnect || _serialPort == null)
        {
            return;
        }

        _serialPort.Write(buffer, offset, count);
    }
}