using Comm.Lib.Interface;

namespace Comm.Service.Service;

public class ScriptManager : IScriptManager
{
    public string RootPath => Path.Combine(Directory.GetCurrentDirectory(), "scripts");
    public string CommonPath => Path.Combine(RootPath, "common");

    public async Task<string> GetScriptContent(string type, string name)
    {
        var path = GetPathByScriptType(type);
        var filePath = Path.Combine(path, name + ".js");
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        return await File.ReadAllTextAsync(filePath);
    }


    public Task EditScript(string type, string name, string content)
    {
        var path = GetPathByScriptType(type);
        var filePath = Path.Combine(path, name + ".js");
        return File.WriteAllTextAsync(filePath, content);
    }

    public void RemoveScript(string type, string name)
    {
        var path = GetPathByScriptType(type);
        var filePath = Path.Combine(path, name + ".js");
        File.Delete(filePath);
    }

    public string GetTemplate(string type)
    {
        var fileName = Path.Combine(CommonPath, $"template_{type}.js");
        return File.Exists(fileName) ? File.ReadAllText(fileName) : "";
    }

    public string GetPathByScriptType(string type)
    {
        if (!Path.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }

        var scriptPath = Path.Combine(RootPath, type);
        if (!Path.Exists(scriptPath))
        {
            Directory.CreateDirectory(scriptPath);
        }


        return scriptPath;
    }

    public List<string> GetScriptNames(string type)
    {
        var path = GetPathByScriptType(type);
        return Directory.GetFiles(path)
            .Where(it => Path.GetExtension(it) == ".js")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(it => !string.IsNullOrEmpty(it))
            .Select(it => it!)
            .ToList();
    }
}