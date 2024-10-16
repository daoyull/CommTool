using System.Text.Json.Serialization;

namespace NetTool.Lib.Interface;

/// <summary>
/// 文本展示接口
/// </summary>
public interface IContentUi
{
    
    /// <summary>
    /// 追加接收区文本
    /// </summary>
    /// <param name="text">文本</param>
    void AppendText(string text);


}