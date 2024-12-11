using System.ComponentModel;
using Common.Lib.Ioc;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.ClearScript.V8;
using NetTool.Abstracts.Plugins;
using NetTool.Common;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Share;

namespace NetTool.Abstracts;

public abstract partial class AbstractNetViewModel<T> : BaseViewModel where T : IMessage
{
    public AbstractNetViewModel()
    {
        RefreshScriptSource();
        AddPlugin<EventRegisterPlugin<T>>();
        AddPlugin<ReceiveScriptPlugin<T>>();
        AddPlugin<SendScriptPlugin<T>>();
    }

    protected INotify Notify => Ioc.Resolve<INotify>();

    protected IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();

    protected IScriptManager ScriptManager => Ioc.Resolve<IScriptManager>();

    [ObservableProperty] private bool _isConnect;

    public ISendOption SendOption => Communication.SendOption;

    public IReceiveOption ReceiveOption => Communication.ReceiveOption;

    public INetUi Ui { get; set; } = null!;


    public abstract ICommunication<T> Communication { get; }
    


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
    

    protected internal virtual void InitCommunication()
    {
        Communication.Connected += HandleConnected;
        Communication.Closed += HandleClosed;
        if (SendOption is ObservableObject observableSendOption)
        {
            observableSendOption.PropertyChanged += HandleSendOptionChanged;
        }
    }

    private CancellationTokenSource? _autoSendCts;

    private void HandleSendOptionChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SendOption.AutoSend))
        {
            if (SendOption.AutoSend)
            {
                _autoSendCts = new();
                Task.Run(AutoSendMethod, _autoSendCts.Token);
            }
            else
            {
                _autoSendCts?.Cancel();
                _autoSendCts?.Dispose();
                _autoSendCts = null;
            }
        }

        if (e.PropertyName == nameof(SendOption.IsHex))
        {
            if (e.PropertyName == nameof(SendOption.IsHex))
            {
                var message = Ui.SendMessage;
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                string msg;
                if (SendOption.IsHex)
                {
                    msg = message.StringToHexString();
                }
                else
                {
                    msg = message.HexStringToString();
                }

                Ui.SendMessage = msg;
            }
        }
    }

    protected virtual async Task? AutoSendMethod()
    {
        while (IsConnect && SendOption.AutoSend && _autoSendCts is { IsCancellationRequested: false })
        {
            if (Ui != null)
            {
                var message = Ui.SendMessage;
                await SendMessage(message);
            }

            await Task.Delay(SendOption.AutoSendTime);
        }
    }

    private void HandleClosed(object? sender, ClosedArgs e)
    {
        IsConnect = false;
        // 关闭消息处理清空已存在的消息
        _receiveCts?.Cancel();
        _receiveCts?.Dispose();
        _receiveCts = null;
    }

    private CancellationTokenSource? _receiveCts;

    private void HandleConnected(object? sender, ConnectedArgs e)
    {
        IsConnect = true;
        // 开启消息处理
        _receiveCts = new();
        Task.Run(StartHandleReceive, _receiveCts.Token);
    }

    protected virtual async void StartHandleReceive()
    {
        try
        {
            while (IsConnect && _receiveCts is { IsCancellationRequested: false })
            {
                var message = await Communication.MessageReadAsync(_receiveCts.Token);
                // 收到数据帧和次数增加
                Ui.AddReceiveFrame(1);
                Ui.AddReceiveBytes((uint)message.Data.Length);

                // 解析收到的数据
                string receiveMessage;
                if (ReceiveOption.IsHex)
                {
                    receiveMessage = message.Data.BytesToHexString();
                }
                else
                {
                    receiveMessage = message.Data.BytesToString();
                }

                // 不输出到界面
                if (!ReceiveOption.DefaultWriteUi)
                {
                    continue;
                }

                HandleReceiveMessage(message, receiveMessage);

                // 自动换行
                if (ReceiveOption.AutoNewLine)
                {
                    Ui.Logger.Write(string.Empty, string.Empty);
                }
                
            }
        }
        catch (OperationCanceledException)
        {
        }
    }


    protected abstract void HandleReceiveMessage(T message, string strMessage);
    protected abstract void HandleSendMessage(byte[] bytes, string message);

    protected virtual bool SendCheck(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            Notify.Warning("发送内容不可为空");
            return false;
        }

        return true;
    }

    [RelayCommand]
    private async Task Send(string message)
    {
        if (!SendCheck(message))
        {
            return;
        }

        await SendMessage(message);
    }

    private async Task SendMessage(string message)
    {
        byte[] buffer;
        if (SendOption.IsHex)
        {
            buffer = message.HexStringToBytes();
        }
        else
        {
            buffer = GlobalOption.Encoding.GetBytes(message);
        }

        var canSend = await HandleSendBytes(buffer);
        if (!canSend)
        {
            return;
        }

        string uiMessage;
        if (SendOption.IsHex)
        {
            uiMessage = buffer.BytesToHexString();
        }
        else
        {
            uiMessage = buffer.BytesToString();
        }

        HandleSendMessage(buffer, uiMessage);

        // 自动换行
        if (ReceiveOption.AutoNewLine)
        {
            Ui.Logger.Write(string.Empty, string.Empty);
        }
    }

    protected virtual async Task<bool> HandleSendBytes(byte[] buffer)
    {
        await Communication.WriteAsync(buffer, 0, buffer.Length);
        Ui.AddSendFrame(1);
        Ui.AddSendBytes((uint)buffer.Length);
        return true;
    }
}

/// <summary>
/// 脚本部分
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract partial class AbstractNetViewModel<T>
{
    public abstract string ScriptType { get; }

    public string ReceiveScriptType => ScriptType + "Receive";
    public string SendScriptType => ScriptType + "Send";

    [ObservableProperty] private List<string>? _receiveScriptSource;
    [ObservableProperty] private List<string>? _sendScriptSource;
    [ObservableProperty] private string? _selectedReceiveScript;
    [ObservableProperty] private string? _selectedSendScript;


    public void RefreshScriptSource()
    {
        ReceiveScriptSource = ScriptManager.GetScriptNames(ReceiveScriptType);
        SendScriptSource = ScriptManager.GetScriptNames(SendScriptType);
        if (!string.IsNullOrEmpty(SelectedReceiveScript) && !ReceiveScriptSource.Contains(SelectedReceiveScript))
        {
            SelectedReceiveScript = null;
        }

        if (!string.IsNullOrEmpty(SelectedSendScript) && !SendScriptSource.Contains(SelectedSendScript))
        {
            SelectedSendScript = null;
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


    internal virtual void LoadEngine(V8ScriptEngine engine)
    {
        engine.AddHostObject("notify", Notify);
        engine.AddHostObject("Communication", this);
        engine.AddHostObject("area", Ui.Logger);
    }
}