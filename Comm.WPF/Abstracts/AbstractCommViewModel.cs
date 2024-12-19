using System.ComponentModel;
using Common.Lib.Ioc;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comm.Lib.Args;
using Comm.Lib.Interface;
using Comm.Service.Share;
using Comm.WPF.Servcice;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T> : BaseViewModel where T : IMessage
{
    #region 属性

    protected INotify Notify => Ioc.Resolve<INotify>();

    protected IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();
    

    [ObservableProperty] private bool _isConnect;


    public ISendOption SendOption => Communication.SendOption;

    public IReceiveOption ReceiveOption => Communication.ReceiveOption;

    public ICommUi Ui { get; set; } = null!;


    public abstract ICommunication<T> Communication { get; }

    #endregion

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


    protected abstract void LogReceiveMessage(T message, string strMessage);
    protected abstract void LogSendMessage(byte[] bytes, string message);
    protected abstract void InvokeSendScript(byte[] buffer, string uiMessage);


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

    public byte[] StringToBuffer(string message)
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

        return buffer;
    }

    public string BufferToString(byte[] buffer)
    {
        if (SendOption.IsHex)
        {
            return buffer.BytesToHexString();
        }

        return buffer.BytesToString();
    }

    private async Task SendMessage(string message)
    {
        try
        {
            var buffer = StringToBuffer(message);
            string uiMessage;
            if (SendOption.IsHex)
            {
                uiMessage = buffer.BytesToHexString();
            }
            else
            {
                uiMessage = buffer.BytesToString();
            }

            LogSendMessage(buffer, uiMessage);
            InvokeSendScript(buffer, uiMessage);
            // 自动换行
            if (ReceiveOption.AutoNewLine)
            {
                Ui.Logger.Write(string.Empty, string.Empty);
            }

            await HandleSendBytes(buffer);
        }
        catch (Exception e)
        {
            Ui.Logger.Error(e.Message);
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