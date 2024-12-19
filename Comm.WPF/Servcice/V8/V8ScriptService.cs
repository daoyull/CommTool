using System.ComponentModel;
using Comm.Lib.Interface;
using Common.Lib.Exceptions;
using Common.Lib.Ioc;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Servcice.V8;

public class V8ScriptService
{
    private readonly IScriptOption _scriptOption;

    private readonly string _type;
    public bool IsLoad { get; private set; }

    private readonly object _scriptLock = new();

    public Action<V8ScriptEngine>? LoadEngine { get; set; }

    private INotify Notify { get; } = Ioc.Resolve<INotify>();
    private IScriptManager ScriptManager => Ioc.Resolve<IScriptManager>();
    public V8ScriptEngine? Engine { get; private set; }
    private IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();

    public V8ScriptService(IScriptOption scriptOption, string type)
    {
        _scriptOption = scriptOption;
        _type = type;
        if (scriptOption is not ObservableObject observableObject)
        {
            throw new BusinessException("Script Object Is Not ObservableObject");
        }

        observableObject.PropertyChanged += HandlePropertyChanged;
    }

    private async void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IScriptOption.IsEnableScript))
        {
            if (_scriptOption.IsEnableScript)
            {
                await StartScript();
            }
            else
            {
                StopScript();
            }
        }
    }
    

    private async Task StartScript()
    {
        // 启动脚本
        // 查找脚本内容
        if (string.IsNullOrEmpty(_scriptOption.ScriptName))
        {
            Notify.Warning("请先选择脚本");
            _scriptOption.IsEnableScript = false;
            return;
        }
        

      
        var scriptContent =
            await ScriptManager.GetScriptContent(_type, _scriptOption.ScriptName!);
      
        if (string.IsNullOrEmpty(scriptContent))
        {
            Notify.Warning("脚本内容为空");
            _scriptOption.IsEnableScript = false;
            return;
        }

        lock (_scriptLock)
        {
            Engine?.Dispose();
            V8ScriptEngineFlags flags;
            if (_scriptOption.IsEnableScriptDebug)
            {
                flags = V8ScriptEngineFlags.EnableDebugging
                        | V8ScriptEngineFlags.EnableDateTimeConversion
                        | V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart;
            }
            else
            {
                flags = V8ScriptEngineFlags.EnableDateTimeConversion;
            }

            Engine = new V8ScriptEngine(flags, GlobalOption.ReceiveScriptDebugPort);
            LoadEngine?.Invoke(Engine);
            IsLoad = true;
            Engine.Execute(scriptContent);
        }
    }

    public void StopScript()
    {
        // 停止脚本
        lock (_scriptLock)
        {
            Engine?.Dispose();
            IsLoad = false;
        }
    }
}