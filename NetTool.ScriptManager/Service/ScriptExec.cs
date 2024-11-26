using Microsoft.ClearScript.V8;
using NetTool.Lib.Interface;
using NetTool.ScriptManager.Interface;

namespace NetTool.ScriptManager.Service;

public class ScriptExec : IScriptExec
{
    private readonly INotify _notify;

    public ScriptExec(INotify notify)
    {
        _notify = notify;
    }

    private V8ScriptEngine? _engine;
    private bool _isLoad;
    private readonly object _lock = new();
    private string? _script;

    public bool Loaded => _isLoad;

    public void Reload(string script, Action<V8ScriptEngine>? initAction = null)
    {
        lock (_lock)
        {
            _engine?.Dispose();
            _engine = new V8ScriptEngine();
            initAction?.Invoke(_engine);
            _script = script;
            _isLoad = true;
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

            _engine!.Execute(_script);
        }
    }

    public void Unload()
    {
        lock (_lock)
        {
            _engine?.Dispose();
            _isLoad = false;
        }
    }

    public void Dispose()
    {
        Unload();
    }
}