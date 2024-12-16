﻿using Comm.Lib.Args;
using Comm.Service.Share;

namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T>
{
    private CancellationTokenSource? _receiveCts;

    protected virtual void InitConnectState()
    {
        Communication.Connected += HandleConnected;
        Communication.Closed += HandleClosed;
    }

    private void HandleConnected(object? sender, ConnectedArgs e)
    {
        IsConnect = true;
        // 开启消息处理
        _receiveCts = new();
        Task.Run(StartHandleReceive, _receiveCts.Token);
    }


    private void HandleClosed(object? sender, ClosedArgs e)
    {
        IsConnect = false;
        // 关闭消息处理清空已存在的消息
        _receiveCts?.Cancel();
        _receiveCts?.Dispose();
        _receiveCts = null;
    }

    protected async void StartHandleReceive()
    {
        try
        {
            while (IsConnect && _receiveCts is { IsCancellationRequested: false })
            {
                var message = await Communication.MessageReadAsync(_receiveCts.Token);
                try
                {
                    bool logHandle = false;
                    bool frameHandle = false;
                    OnReceiveScript(message, ref logHandle, ref frameHandle);
                    if (!frameHandle)
                    {
                        // 收到数据帧和次数增加
                        Ui.AddReceiveFrame(1);
                        Ui.AddReceiveBytes((uint)message.Data.Length);
                    }

                    if (ReceiveOption.DefaultWriteUi && !logHandle)
                    {
                        // 解析收到的数据
                        var receiveMessage = ReceiveOption.IsHex
                            ? message.Data.BytesToHexString()
                            : message.Data.BytesToString();

                        LogReceiveMessage(message, receiveMessage);
                        // 自动换行
                        if (ReceiveOption.AutoNewLine)
                        {
                            Ui.Logger.Write(string.Empty, string.Empty);
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
}