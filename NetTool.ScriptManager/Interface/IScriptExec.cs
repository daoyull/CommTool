using Microsoft.ClearScript.V8;

namespace NetTool.ScriptManager.Interface;

public interface IScriptExec : IDisposable
{
    public V8ScriptEngine? Engine { get;  }
    public bool Loaded { get; }
    void Reload(string script,Action<V8ScriptEngine>? initAction = null);

    void Invoke(Action<V8ScriptEngine> invokeAction );

    void Unload();
}