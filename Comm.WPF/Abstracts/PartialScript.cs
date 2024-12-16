using System.IO;
using Comm.Lib.Interface;
using Comm.WPF.Common;
using Comm.WPF.Servcice;
using Common.Lib.Ioc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T>
{
    protected abstract string ScriptType { get; }

    protected V8ScriptService V8Receive { get; }
    protected V8ScriptService V8Send { get; }

    private IScriptManager ScriptManager { get; } = Ioc.Resolve<IScriptManager>();

    private string ReceiveScriptType => ScriptType + "Receive";
    private string SendScriptType => ScriptType + "Send";

    [ObservableProperty] private List<string>? _receiveScriptSource;
    [ObservableProperty] private List<string>? _sendScriptSource;


    public void RefreshScriptSource()
    {
        ReceiveScriptSource = ScriptManager.GetScriptNames(ReceiveScriptType);
        SendScriptSource = ScriptManager.GetScriptNames(SendScriptType);
        if (!string.IsNullOrEmpty(ReceiveOption.ScriptName) && !ReceiveScriptSource.Contains(ReceiveOption.ScriptName))
        {
            ReceiveOption.ScriptName = null;
        }

        if (!string.IsNullOrEmpty(SendOption.ScriptName) && !SendScriptSource.Contains(SendOption.ScriptName))
        {
            SendOption.ScriptName = null;
        }
    }

    protected virtual string InitReceiveScript =>
        $@"function receive(buffer,time,message){{
    debugger;
    area.Info(`Receive Script Console: ${{message}}`)
}}";

    protected virtual string InitSendScript =>
        $@"function send(buffer,time,message){{
    debugger;
    area.Info(`Send Script Console: ${{message}}`)
}}";

    [RelayCommand]
    private void ShowReceiveScriptManager()
    {
        ScriptDialogHelper.ShowDialog(ReceiveScriptType, InitReceiveScript, this);
        RefreshScriptSource();
    }

    [RelayCommand]
    private void ShowSendScriptManager()
    {
        ScriptDialogHelper.ShowDialog(SendScriptType, InitSendScript, this);
        RefreshScriptSource();
    }


    private void LoadEngine(V8ScriptEngine engine)
    {
        // 加载common目录下的所有脚本
        var commonPath = Path.Combine(GlobalOption.ScriptPath, "common");
        if (Directory.Exists(commonPath))
        {
            foreach (var file in Directory.GetFiles(commonPath))
            {
                if (file.EndsWith(".js"))
                {
                    string scriptContent = File.ReadAllText(file);
                    engine.Execute(scriptContent);
                }
            }
        }

        engine.AddHostObject("notify", Notify);
        engine.AddHostObject("comm", new JsComm<T>(this));
        engine.AddHostObject("area", Ui.Logger);
    }

    protected virtual void InitScript()
    {
        V8Receive.LoadEngine += LoadEngine;
        V8Send.LoadEngine += LoadEngine;
    }

    private void OnReceiveScript(T message, ref bool logHandle, ref bool frameHandle)
    {
        if (!IsConnect || !V8Receive.IsLoad)
        {
            return;
        }

        dynamic result = InvokeReceiveScript(message);
        if (result == Undefined.Value)
        {
            return;
        }

        if (result.logHandle != Undefined.Value)
        {
            bool.TryParse((string)result.logHandle.ToString(), out logHandle);
        }

        if (result.frameHandle != Undefined.Value)
        {
            bool.TryParse((string)result.frameHandle.ToString(), out frameHandle);
        }
    }
}