namespace Comm.Lib.Interface;

public interface ICommUi
{
    /// <summary>
    /// 全局选项
    /// </summary>
    public IGlobalOption GlobalOption { get; }

    /// <summary>
    /// 界面日志区域
    /// </summary>
    public IUiLogger Logger { get; }

    /// <summary>
    /// 消息通知接口
    /// </summary>
    public INotify Notify { get; }

    /// <summary>
    /// 发送区域选项
    /// </summary>
    public ISendOption SendOption { get; set; }

    /// <summary>
    /// 接收区域选项
    /// </summary>
    public IReceiveOption ReceiveOption { get; set; }

    /// <summary>
    /// 增加发送帧计数
    /// </summary>
    /// <param name="add"></param>
    public void AddSendFrame(uint add);

    /// <summary>
    /// 增加接收帧计数
    /// </summary>
    /// <param name="add"></param>
    public void AddReceiveFrame(uint add);

    /// <summary>
    /// 增加发送字节计数
    /// </summary>
    /// <param name="add"></param>
    public void AddSendBytes(uint add);

    /// <summary>
    /// 增加接收字节计数
    /// </summary>
    /// <param name="add"></param>
    public void AddReceiveBytes(uint add);

    /// <summary>
    /// 重置计数
    /// </summary>
    public void ResetNumber();

    /// <summary>
    /// 滚动到底部
    /// </summary>
    public void ScrollToEnd();

    /// <summary>
    /// 界面日志的内容
    /// </summary>
    public string ReceiveMessage { get; }

    /// <summary>
    /// 发送区域内容
    /// </summary>
    public string SendMessage { get; set; }
}