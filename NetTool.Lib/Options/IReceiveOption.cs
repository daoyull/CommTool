namespace NetTool.Lib.Interface;

public interface IReceiveOption
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
    /// 脚本名称
    /// </summary>
    public string? ScriptName { get; set; }
}