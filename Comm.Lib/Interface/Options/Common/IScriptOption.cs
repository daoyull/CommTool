namespace Comm.Lib.Interface;

public interface IScriptOption : IOption
{
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
}