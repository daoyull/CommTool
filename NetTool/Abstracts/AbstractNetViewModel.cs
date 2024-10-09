using System.ComponentModel;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Module.Share;

namespace NetTool.Abstracts;

public abstract partial class AbstractNetViewModel<T> : BaseViewModel where T : IMessage
{
    protected INotify Notify { get; }
    protected IGlobalOption GlobalOption { get; }

    public AbstractNetViewModel(INotify notify, IGlobalOption globalOption)
    {
        Notify = notify;
        GlobalOption = globalOption;
    }

    [ObservableProperty] private bool _isConnect;

    public ISendOption SendOption => Communication.SendOption;

    public IReceiveOption ReceiveOption => Communication.ReceiveOption;

    public INetUi? Ui { get; set; }


    public abstract ICommunication<T> Communication { get; }


    [RelayCommand]
    protected abstract Task Connect();

    protected virtual void InitCommunication()
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
                var message = Ui!.SendMessage;
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
                Ui?.AddReceiveFrame(1);
                Ui?.AddReceiveBytes((uint)message.Data.Length);
                string receiveMessage;
                if (ReceiveOption.IsHex)
                {
                    receiveMessage = message.Data.ToHexString();
                }
                else
                {
                    receiveMessage = GlobalOption.Encoding.GetString(message.Data);
                }

                HandleReceiveMessage(message, receiveMessage);
            }
        }
        catch (OperationCanceledException _)
        {
        }
    }

    protected abstract void HandleReceiveMessage(T message, string strMessage);
    protected abstract void HandleSendMessage(string message);

    [RelayCommand]
    private async Task Send(string message)
    {
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
            uiMessage = buffer.ToHexString();
        }
        else
        {
            uiMessage = GlobalOption.Encoding.GetString(buffer);
        }

        HandleSendMessage(uiMessage);
    }

    protected virtual async Task<bool> HandleSendBytes(byte[] buffer)
    {
        await Communication.WriteAsync(buffer, 0, buffer.Length);
        Ui?.AddSendFrame(1);
        Ui?.AddSendBytes((uint)buffer.Length);
        return true;
    }
}