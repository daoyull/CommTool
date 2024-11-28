namespace NetTool.ScriptManager.Interface;

public interface IScriptManager
{
    public string RootPath { get; }

    public Task<string> GetScriptContent(string type, string name);

    public Task EditScript(string type, string name, string content);

    public void RemoveScript(string type, string name);

    public string GetPathByScriptType(string type);
    public List<string> GetScriptNames(string type);
}