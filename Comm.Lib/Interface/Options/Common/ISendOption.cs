namespace Comm.Lib.Interface;

/// <summary>
/// 发送选项
/// </summary>
public interface ISendOption
{
    /// <summary>
    /// 发送信息是否输出到界面
    /// </summary>
    public bool DefaultWriteUi { get; set; }

    /// <summary>
    /// 是否16进制发送
    /// </summary>
    public bool IsHex { get; set; }

    /// <summary>
    /// 是否启用脚本
    /// </summary>
    public bool IsEnableScript { get; set; }

    /// <summary>
    /// 是否启用脚本调试
    /// </summary>
    public bool IsEnableScriptDebug { get; set; }

    /// <summary>
    /// 脚本名称
    /// </summary>    
    public string? ScriptName { get; set; }

    /// <summary>
    /// 是否自动发送
    /// </summary>
    public bool AutoSend { get; set; }

    /// <summary>
    /// 自动发送时间间隔
    /// </summary>
    public int AutoSendTime { get; set; }
}