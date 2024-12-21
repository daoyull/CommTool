using System.IO;
using Comm.Lib.Args;
using Common.Lib.Ioc;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comm.Lib.Interface;
using Comm.WPF.Servcice.V8;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T> : BaseViewModel where T : IMessage
{
    #region 字段

    private CancellationTokenSource? _receiveCts;

    [ObservableProperty] private bool _isConnect;

    #endregion

    #region 属性

    protected abstract string Type { get; }

    protected IScriptManager ScriptManager { get; } = Ioc.Resolve<IScriptManager>();

    protected INotify Notify => Ioc.Resolve<INotify>();

    protected IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();

    public ICommUi Ui { get; set; } = null!;

    public abstract ICommunication<T> Communication { get; }

    #endregion

    #region Command

    /// <summary>
    /// 连接
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    protected virtual Task Connect()
    {
        try
        {
            if (IsConnect)
            {
                Communication.Close();
                Communication.Dispose();
            }
            else
            {
                Communication.Connect();
            }
        }
        catch (Exception e)
        {
            Notify.Error(e.Message);
        }

        return Task.CompletedTask;
    }

    #endregion

    #region Method

    /// <summary>
    /// 日志输出接收到信息
    /// </summary>
    /// <param name="message"></param>
    protected abstract void LogUiReceiveMessage(T message);

    /// <summary>
    /// 文件日志输出接收到信息
    /// </summary>
    /// <param name="message"></param>
    protected abstract void LogFileReceiveMessage(T message);

    /// <summary>
    /// 日志输出发送信息
    /// </summary>
    /// <param name="bytes"></param>
    protected abstract void LogSendMessage(byte[] bytes);

    /// <summary>
    /// 初始化方法
    /// </summary>
    protected virtual void InitCommunication()
    {
        RefreshScriptSource();
        InitScript();
        InitConnectState();
        if (SendOption is ObservableObject observableSendOption)
        {
            observableSendOption.PropertyChanged += HandleSendOptionChanged;
        }
    }

    /// <summary>
    /// 刷新脚本列表
    /// </summary>
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

    /// <summary>
    /// 加载脚本基础对象和方法
    /// </summary>
    /// <param name="engine"></param>
    private void LoadEngine(V8ScriptEngine engine)
    {
        // 加载common目录下的所有脚本
        var commonPath = Path.Combine(GlobalOption.ScriptPath, "common", "common.js");
        if (File.Exists(commonPath))
        {
            string scriptContent = File.ReadAllText(commonPath);
            engine.Execute(scriptContent);
        }

        engine.AddHostObject("notify", new JsNotify<T>(this));
        engine.AddHostObject("comm", new JsComm<T>(this));
        engine.AddHostObject("ui", new JsUi<T>(this));
        engine.AddHostObject("util", new JsUtil(engine));
    }

    /// <summary>
    /// 初始化脚本方法
    /// </summary>
    protected virtual void InitScript()
    {
        V8Receive = new V8ScriptService(ReceiveOption, ReceiveScriptType);
        V8Send = new V8ScriptService(SendOption, SendScriptType);
        V8Receive.LoadEngine += LoadEngine;
        V8Send.LoadEngine += LoadEngine;
    }

    /// <summary>
    /// 初始化连接状态
    /// </summary>
    protected virtual void InitConnectState()
    {
        Communication.Connected += HandleConnected;
        Communication.Closed += HandleClosed;
    }

    /// <summary>
    /// 处理连胜成功
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleConnected(object? sender, ConnectedArgs e)
    {
        IsConnect = true;
        // 开启消息处理
        _receiveCts = new();
        Task.Run(StartHandleReceive, _receiveCts.Token);
    }

    /// <summary>
    /// 处理关闭连接
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleClosed(object? sender, ClosedArgs e)
    {
        IsConnect = false;
        // 关闭消息处理清空已存在的消息
        _receiveCts?.Cancel();
        _receiveCts?.Dispose();
        _receiveCts = null;
    }

    #endregion
}