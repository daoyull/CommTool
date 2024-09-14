using Microsoft.Extensions.Options;
using NetTool.Lib.Entity;

namespace NetTool.Module.Service;

public class ScriptManager
{
    private readonly NetConfig _netConfig;

    public ScriptManager(IOptions<NetConfig> configOption)
    {
        _netConfig = configOption.Value;
    }

    public List<string> GetScriptList()
    {
        var list = new List<string>();
        var directory = _netConfig.ScriptDirectory;
        if (string.IsNullOrEmpty(directory))
        {
            return  list;
        }
        if (!Directory.Exists(directory))
        {
            return list;
        }
        return Directory.GetFiles(directory, "*.js").Select(Path.GetFileName).ToList();
    }
}