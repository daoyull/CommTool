namespace Comm.Lib.Interface;

public interface IScriptManager
{
    /// <summary>
    /// 脚本根目录
    /// </summary>
    public string RootPath { get; }

    /// <summary>
    /// 获取脚本内容
    /// </summary>
    public Task<string> GetScriptContent(string type, string name);

    /// <summary>
    /// 编辑脚本
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public Task EditScript(string type, string name, string content);

    /// <summary>
    /// 删除掉本
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    public void RemoveScript(string type, string name);

    /// <summary>
    /// 获取对应类型的脚本目录地址
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetPathByScriptType(string type);

    /// <summary>
    /// 获取对应类型的全部脚本
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<string> GetScriptNames(string type);
}