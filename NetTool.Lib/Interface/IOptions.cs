using System.IO.Ports;
using System.Text;

namespace NetTool.Lib.Interface;

/// <summary>
/// 全局配置
/// </summary>
public interface IGlobalOption
{
    /// <summary>
    /// 缓冲区大小
    /// </summary>
    public int BufferSize { get; set; }

    /// <summary>
    /// 脚本根目录
    /// </summary>
    public string ScriptPath { get; set; }

    public Encoding Encoding { get; set; }
}

public interface IReceiveOption
{
    public bool DefaultWriteUi { get; set; }

    public bool SaveToFile { get; set; }

    public bool IsHex { get; set; }

    public bool AutoNewLine { get; set; }

    public bool IsEnableScript { get; set; }

    public string? ScriptName { get; set; }
}

public interface ISendOption
{
    public bool DefaultWriteUi { get; set; }

    public bool IsHex { get; set; }

    public bool IsEnableScript { get; set; }

    public string? ScriptName { get; set; }

    public bool AutoSend { get; set; }

    public int AutoSendTime { get; set; }
}

#region SerialPort

public interface ISerialOption
{
    public string? SerialPortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits? StopBits { get; set; }

    public Parity? Parity { get; set; }
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

public interface ITcpClientReceiveOption : IReceiveOption
{
    public bool AutoBreakFrame { get; set; }

    public int AutoBreakFrameTime { get; set; }
}

public interface ITcpClientSendOption : ISendOption;

#endregion

#region TcpServer

public interface ITcpServerOption
{
    public string? Ip { get; set; }
    public int Port { get; set; }
}

public interface ITcpServerReceiveOption : IReceiveOption
{
    public bool AutoBreakFrame { get; set; }

    public int AutoBreakFrameTime { get; set; }
}

public interface ITcpServerSendOption : ISendOption;

#endregion