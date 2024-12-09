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
    public int ScriptDebugPort { get; set; }
}