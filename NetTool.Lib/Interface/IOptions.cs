using System.IO.Ports;

namespace NetTool.Lib.Interface;

public interface ISerialOption
{
    public string? SerialPortName { get; set; }

    public int BaudRate { get; set; }

    public int DataBits { get; set; }

    public StopBits? StopBits { get; set; }

    public Parity? Parity { get; set; }
}

public interface IReceiveOption
{
    public bool DefaultWriteUi { get; set; }

    public bool SaveToFile { get; set; }

    public bool IsHex { get; set; }

    public bool AutoBreakFrame { get; set; }

    public int AutoBreakFrameTime { get; set; }

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

public interface ISerialReceiveOption : IReceiveOption;

public interface ISerialSendOption : ISendOption;