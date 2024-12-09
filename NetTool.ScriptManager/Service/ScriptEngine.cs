using Microsoft.ClearScript.V8;
using NetTool.Lib.Interface;
using NetTool.ScriptManager.Interface;

namespace NetTool.ScriptManager.Service;

public class ScriptEngine : IScriptExec
{
    private readonly INotify _notify;

    public ScriptEngine(INotify notify)
    {
        _notify = notify;
    }


    private bool _isLoad;
    private readonly object _lock = new();
    private string? _script;

    public V8ScriptEngine? Engine { get; private set; }
    public bool Loaded => _isLoad;

    public void Reload(string script, Action<V8ScriptEngine>? initAction = null)
    {
        lock (_lock)
        {
            Engine?.Dispose();
            Engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging
                                        | V8ScriptEngineFlags.EnableDateTimeConversion
                                        | V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart, 9901);
            initAction?.Invoke(Engine);
            _script = script;
            _isLoad = true;
            Engine.Execute(_script);
        }
    }

    public void Invoke(Action<V8ScriptEngine> invokeAction)
    {
        lock (_lock)
        {
            if (!_isLoad)
            {
                _notify.Warning($"脚本未加载，请重试");
                return;
            }

            Engine!.Execute(_script);
        }
    }

    public void Unload()
    {
        lock (_lock)
        {
            Engine?.Dispose();
            _isLoad = false;
        }
    }

    public void Dispose()
    {
        Unload();
    }
}