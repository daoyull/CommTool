namespace NetTool.Lib.Interface;

public interface ITcpClientService : INetService
{
    public int ReceiveBufferSize { get; set; }

    /// <summary>
    /// 自动断帧时间
    /// </summary>
    public long AutoBreakTime { get; set; }

    /// <summary>
    /// 是否自动断帧
    /// </summary>
    public bool AutoBreak { get; set; }

    public string? Ip { get; set; }

    public int Port { get; set; }

    /// <summary>
    /// 连接
    /// </summary>
    /// <returns></returns>
    Task ConnectAsync();

    /// <summary>
    /// 关闭连接
    /// </summary>
    /// <returns></returns>
    Task CloseAsync();

    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    Task SendAsync(byte[] buffer);
}