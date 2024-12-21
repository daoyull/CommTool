using System.ComponentModel;
using Comm.Lib.Interface;
using Comm.Service.Share;
using Comm.WPF.Common;
using Comm.WPF.Servcice.V8;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.ClearScript;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T>
{
    #region 字段

    private CancellationTokenSource? _autoSendCts;

    [ObservableProperty] private List<string>? _sendScriptSource;

    #endregion

    #region 属性

    /// <summary>
    /// 发送脚本类型
    /// </summary>
    private string SendScriptType => Type + "Send";

    /// <summary>
    /// 发送脚本模板
    /// </summary>
    private string InitSendScript => ScriptManager.GetTemplate(SendScriptType);

    /// <summary>
    /// 发送选项
    /// </summary>
    public ISendOption SendOption => Communication.SendOption;

    /// <summary>
    /// 发送脚本引擎服务
    /// </summary>
    protected V8ScriptService V8Send { get; private set; } = null!;

    #endregion

    #region Command

    /// <summary>
    /// 打开编辑发送脚本页面
    /// </summary>
    [RelayCommand]
    private void ShowSendScriptManager()
    {
        ScriptDialogHelper.ShowDialog(SendScriptType, InitSendScript, this);
        RefreshScriptSource();
    }

    /// <summary>
    /// 发送信息Command
    /// </summary>
    /// <param name="message"></param>
    [RelayCommand]
    private async Task Send(string message)
    {
        await SendMessage(message);
    }

    #endregion

    #region Method

    /// <summary>
    /// 文件日志输出
    /// </summary>
    /// <param name="buffer"></param>
    protected abstract void LogFileSendMessage(byte[] buffer);

    /// <summary>
    /// 执行发送脚本
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    protected abstract object InvokeSendScript(byte[] buffer);

    /// <summary>
    /// 处理自动发送变更和IsHex变更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleSendOptionChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SendOption.AutoSend) when SendOption.AutoSend:
                _autoSendCts = new();
                Task.Run(StartAutoSend, _autoSendCts.Token);
                break;
            case nameof(SendOption.AutoSend):
                _autoSendCts?.Cancel();
                _autoSendCts?.Dispose();
                _autoSendCts = null;
                break;
            case nameof(SendOption.IsHex):
            {
                var message = Ui.SendMessage;
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                Ui.SendMessage = SendOption.IsHex ? message.StringToHexString() : message.HexStringToString();
                break;
            }
        }
    }

    /// <summary>
    /// 自动发送Task
    /// </summary>
    protected virtual async Task? StartAutoSend()
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

    /// <summary>
    /// 触发发送脚本
    /// </summary>
    private void OnSendScript(byte[] buffer, ref bool logHandle, ref bool frameHandle, ref bool sendHandle)
    {
        if (!IsConnect || !V8Send.IsLoad)
        {
            return;
        }

        dynamic result = InvokeSendScript(buffer);
        if (Undefined.Value.Equals(result))
        {
            return;
        }

        if (!Undefined.Value.Equals(result.logHandle))
        {
            bool.TryParse((string)result.logHandle.ToString(), out logHandle);
        }

        if (!Undefined.Value.Equals(result.frameHandle))
        {
            bool.TryParse((string)result.frameHandle.ToString(), out frameHandle);
        }

        if (!Undefined.Value.Equals(result.sendHandle))
        {
            bool.TryParse((string)result.sendHandle.ToString(), out sendHandle);
        }
    }

    /// <summary>
    /// 处理发送帧/字节计数
    /// </summary>
    /// <param name="buffer"></param>
    protected virtual void HandleSendFrame(byte[] buffer)
    {
        Ui.AddSendFrame(1);
        Ui.AddSendBytes((uint)buffer.Length);
    }

    /// <summary>
    /// 界面发送信息方法
    /// </summary>
    /// <param name="message"></param>
    public async Task SendMessage(string message)
    {
        try
        {
            if (!SendCheck(message))
            {
                return;
            }


            var buffer = message.StringToBytes(SendOption.IsHex);
            if (ReceiveOption.SaveToFile)
            {
                LogFileSendMessage(buffer);
            }

            // 调用脚本
            bool logHandle = false;
            bool frameHandle = false;
            bool sendHandle = false;

            OnSendScript(buffer, ref logHandle, ref frameHandle, ref sendHandle);

            if (!logHandle && SendOption.DefaultWriteUi)
            {
                LogSendMessage(buffer);
                // 自动换行
                if (ReceiveOption.AutoNewLine)
                {
                    Ui.Logger.Write(string.Empty, string.Empty);
                }
            }

            if (!frameHandle)
            {
                HandleSendFrame(buffer);
            }

            if (!sendHandle)
            {
                await SendBytes(buffer);
            }
        }
        catch (Exception e)
        {
            Ui.Logger.Error(e.Message);
        }
    }


    /// <summary>
    /// 检查要发送的消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected virtual bool SendCheck(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            Notify.Warning("发送内容不可为空");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 实际发送方法
    /// </summary>
    /// <param name="buffer"></param>
    protected virtual async Task SendBytes(byte[] buffer)
    {
        await Communication.WriteAsync(buffer, 0, buffer.Length);
    }

    #endregion
}