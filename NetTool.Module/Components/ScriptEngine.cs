using Microsoft.ClearScript.V8;
using NetTool.Lib.Entity;
using NetTool.Lib.Interface;
using NetTool.Module.Share;
using Newtonsoft.Json;

namespace NetTool.Module.Components;

public class ScriptEngine : IScript
{
    private V8ScriptEngine _engine = new();
    private string? _script;

    public ScriptEngine()
    {
        _engine.AddHostType("Console", typeof(Console));
        _engine.AddHostType("ByteHelper", typeof(ByteHelper));
    }


    public void Reload(string script)
    {
        _script = script;
        _engine.Dispose();
        _engine = new();
        _engine.Execute(script);
    }


    public void Dispose()
    {
        _engine.Dispose();
    }
}