using System.IO.Ports;
using System.Text;

namespace NetTool.Lib.Interface;

public interface ISocketReceiveOption : IReceiveOption
{
    public bool AutoBreakFrame { get; set; }

    public int AutoBreakFrameTime { get; set; }
}

#region SerialPort

public interface ISerialConnectOption
{
    public string? SerialPortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits? StopBits { get; set; }

    public Parity? Parity { get; set; }

    public bool CanEditBaudRate { get; set; } 
}

public interface ISerialReceiveOption : IReceiveOption;

public interface ISerialSendOption : ISendOption;

#endregion

#region TcpClient

public interface ITcpClientOption
{
    public string? Ip { get; set; }
    public int Port { get; set; }
}

public interface ITcpClientReceiveOption : ISocketReceiveOption
{
}

public interface ITcpClientSendOption : ISendOption;

#endregion

#region TcpServer

public interface ITcpServerOption
{
    public string? Ip { get; set; }
    public int Port { get; set; }
}

public interface ITcpServerReceiveOption : ISocketReceiveOption
{
}

public interface ITcpServerSendOption : ISendOption;

#endregion