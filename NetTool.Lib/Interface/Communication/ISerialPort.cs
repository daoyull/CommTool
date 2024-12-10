namespace NetTool.Lib.Interface;

public interface ISerialPort
{
    /// <summary>
    /// 串口连接选项
    /// </summary>
    public ISerialConnectOption SerialConnectOption { get; }

    /// <summary>
    /// 串口接收选项
    /// </summary>
    public ISerialReceiveOption SerialReceiveOption { get; }

    /// <summary>
    /// 串口发送选项
    /// </summary>
    public ISerialSendOption SerialSendOption { get; }
    
    /// <summary>
    /// 获取全部串口名称
    /// </summary>
    /// <returns></returns>
    public List<string> GetPortNames();
}