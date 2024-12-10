using Common.Lib.Ioc;
using Common.Lib.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.ClearScript.V8;
using NetTool.Lib.Interface;

namespace NetTool.Abstracts.Plugins;

internal class SendScriptPlugin<T> : ILifePlugin where T : IMessage
{
    public bool ScriptLoad { get; set; }
    private readonly object _scriptLock = new();
    private AbstractNetViewModel<T> ViewModel { get; set; } = null!;
    private INotify Notify { get; } = Ioc.Resolve<INotify>();
    private IScriptManager ScriptManager => Ioc.Resolve<IScriptManager>();
    private V8ScriptEngine? Engine { get; set; }
    private IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();

    public void InvokeScript(Action<V8ScriptEngine> action)
    {
        if (Engine != null && ScriptLoad)
        {
            action.Invoke(Engine);
        }
    }

    public Task OnCreate(ILifeCycle lifeCycle)
    {
        if (lifeCycle is AbstractNetViewModel<T> viewModel)
        {
            ViewModel = viewModel;
            if (viewModel.SendOption is ObservableObject sendOption)
            {
                sendOption.PropertyChanged += async (sender, e) =>
                {
                    if (sender is not ISendOption option)
                    {
                        return;
                    }

                    if (e.PropertyName == nameof(ISendOption.IsEnableScript))
                    {
                        if (option.IsEnableScript)
                        {
                            await StartScript();
                        }
                        else
                        {
                            StopScript();
                        }
                    }
                };
            }
        }

        return Task.CompletedTask;
    }

    public Task OnInit(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoad(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnUnload(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    private async Task StartScript()
    {
        // 启动脚本
        // 查找脚本内容
        if (string.IsNullOrEmpty(ViewModel.SelectedSendScript))
        {
            Notify.Warning("请先选择脚本");
            ViewModel.SendOption.IsEnableScript = false;
            return;
        }

        var scriptContent =
            await ScriptManager.GetScriptContent(ViewModel.SendScriptType, ViewModel.SelectedSendScript!);

        if (string.IsNullOrEmpty(scriptContent))
        {
            Notify.Warning("脚本内容为空");
            ViewModel.SendOption.IsEnableScript = false;
            return;
        }

        lock (_scriptLock)
        {
            Engine?.Dispose();
            V8ScriptEngineFlags flags;
            if (ViewModel.SendOption.IsEnableScriptDebug)
            {
                flags = V8ScriptEngineFlags.EnableDebugging
                        | V8ScriptEngineFlags.EnableDateTimeConversion
                        | V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart;
            }
            else
            {
                flags = V8ScriptEngineFlags.EnableDateTimeConversion;
            }

            Engine = new V8ScriptEngine(flags, GlobalOption.SendScriptDebugPort);
            ViewModel.LoadEngine(Engine);
            ScriptLoad = true;
            Engine.Execute(scriptContent);
        }
    }

    public void StopScript()
    {
        // 停止脚本
        lock (_scriptLock)
        {
            Engine?.Dispose();
            ScriptLoad = false;
        }
    }
}