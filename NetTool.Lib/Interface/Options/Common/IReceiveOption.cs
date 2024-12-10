namespace NetTool.Lib.Interface;

/// <summary>
/// 接收选项
/// </summary>
public interface IReceiveOption : IOption
{
    /// <summary>
    /// 是否16进制显示
    /// </summary>
    public bool IsHex { get; set; }

    /// <summary>
    /// 是否自动滚屏
    /// </summary>
    public bool AutoScroll { get; set; }

    /// <summary>
    /// 是否输出到界面
    /// </summary>
    public bool DefaultWriteUi { get; set; }

    /// <summary>
    /// 是否保存到文件
    /// </summary>
    public bool SaveToFile { get; set; }

    /// <summary>
    /// 是否自动换行
    /// </summary>
    public bool AutoNewLine { get; set; }

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
    /// 最大分包时间 ms
    /// </summary>
    public int MaxFrameTime { get; set; }

    /// <summary>
    /// 最大分包字节 byte
    /// </summary>
    public int MaxFrameSize { get; set; }

    /// <summary>
    /// 是否按照最大时间分包
    /// </summary>
    public bool IsMaxFrameTime { get; set; }

    /// <summary>
    /// 是否按照日志格式显示
    /// </summary>
    public bool LogStyleShow { get; set; }
}