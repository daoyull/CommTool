using System.Text;

namespace Comm.Lib.Interface;

/// <summary>
/// 全局配置
/// </summary>
public interface IGlobalOption : IOption
{
    /// <summary>
    /// 缓冲区大小
    /// </summary>
    public int BufferSize { get; set; }

    /// <summary>
    /// 脚本根目录
    /// </summary>
    public string ScriptPath { get; set; }

    /// <summary>
    /// 日志输出编码
    /// </summary>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// 接收脚本Debug端口
    /// </summary>
    public int ReceiveScriptDebugPort { get; set; }

    /// <summary>
    /// 发送脚本Debug端口
    /// </summary>
    public int SendScriptDebugPort { get; set; }
}