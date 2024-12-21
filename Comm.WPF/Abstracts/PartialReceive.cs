using System.IO;
using Comm.Lib.Interface;
using Comm.Service.Share;
using Comm.WPF.Common;
using Comm.WPF.Entity;
using Comm.WPF.Servcice;
using Comm.WPF.Servcice.V8;
using Common.Lib.Ioc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T>
{
    #region 字段

    [ObservableProperty] private List<string>? _receiveScriptSource;

    #endregion

    #region 属性

    /// <summary>
    /// 收到的脚本的类型
    /// </summary>
    private string ReceiveScriptType => Type + "Receive";

    /// <summary>
    /// 获取接收脚本模板
    /// </summary>
    private string InitReceiveScript => ScriptManager.GetTemplate(ReceiveScriptType);
    
    /// <summary>
    /// 接收选项
    /// </summary>
    public IReceiveOption ReceiveOption => Communication.ReceiveOption;
    
    /// <summary>
    /// 脚本引擎服务
    /// </summary>
    protected V8ScriptService V8Receive { get; private set; } = null!;

    #endregion

    #region Command

    [RelayCommand]
    private void ShowReceiveScriptManager()
    {
        ScriptDialogHelper.ShowDialog(ReceiveScriptType, InitReceiveScript, this);
        RefreshScriptSource();
    }

    #endregion

    #region Method

    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    protected abstract object InvokeReceiveScript(T message);

    /// <summary>
    /// 开始处理收到的消息
    /// </summary>
    private async void StartHandleReceive()
    {
        try
        {
            while (IsConnect && _receiveCts is { IsCancellationRequested: false })
            {
                var message = await Communication.MessageReadAsync(_receiveCts.Token);
                try
                {
                    if (ReceiveOption.SaveToFile)
                    {
                        LogFileReceiveMessage(message);
                    }
                    // 调用脚本
                    bool logHandle = false;
                    bool frameHandle = false;
                    OnReceiveScript(message, ref logHandle, ref frameHandle);

                    // 接收帧/字节增加操作
                    if (!frameHandle)
                    {
                        // 收到数据帧和次数增加
                        Ui.AddReceiveFrame(1);
                        Ui.AddReceiveBytes((uint)message.Data.Length);
                    }

                    // 是否输出到界面
                    if (ReceiveOption.DefaultWriteUi && !logHandle)
                    {
                        LogUiReceiveMessage(message);
                        // 自动换行
                        if (ReceiveOption.AutoNewLine)
                        {
                            Ui.Logger.WriteEmptyLine();
                        }
                    }
                }
                catch (Exception e)
                {
                    Ui.Logger.Error(e.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    /// <summary>
    /// 触发脚本
    /// </summary>
    /// <param name="message"></param>
    /// <param name="logHandle"></param>
    /// <param name="frameHandle"></param>
    private void OnReceiveScript(T message, ref bool logHandle, ref bool frameHandle)
    {
        if (!IsConnect || !V8Receive.IsLoad)
        {
            return;
        }

        dynamic result = InvokeReceiveScript(message);
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
    }

    #endregion
}