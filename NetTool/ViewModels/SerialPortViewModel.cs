﻿using System.IO.Ports;
using Common.Lib.Helpers;
using Common.Lib.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.IO;
using NetTool.Module.Messages;
using NetTool.Module.Share;


namespace NetTool.ViewModels;

public partial class SerialPortViewModel : AbstractNetViewModel<SerialPortMessage>
{
    public SerialPortAdapter Serial { get; }
    public override ICommunication<SerialPortMessage> Communication { get; }

    protected override Task Connect()
    {
        try
        {
            if (IsConnected)
            {
                Serial.Close();
                Serial.Dispose();
                Notify.Info("已关闭连接");
            }
            else
            {
                Serial.Connect();
                Notify.Success("串口连接成功");
            }

            IsConnected = !IsConnected;
        }
        catch (Exception e)
        {
            // todo
            Notify.Error(e.Message);
        }

        return Task.CompletedTask;
    }

    [ObservableProperty] private bool _isConnected;

    #region 数据源

    /// <summary>
    /// 串口名称
    /// </summary>
    [ObservableProperty] private List<string> _comPortList = null!;

    /// <summary>
    /// 校验位
    /// </summary>
    public List<EnumItem<Parity>> ParitiesSource { get; } = EnumHelper.EnumConvertToList<Parity>();

    /// <summary>
    /// 停止位
    /// </summary>
    public List<EnumItem<StopBits>> StopBitsSource { get; } = EnumHelper.EnumConvertToList<StopBits>();

    /// <summary>
    /// 波特率
    /// </summary>
    public List<int> BaudRates { get; } = new()
    {
        9600, 19200, 38400, 57600, 115200
    };

    /// <summary>
    /// 数据位
    /// </summary>
    public List<int> DataBits { get; } = new() { 5, 6, 7, 8 };

    #endregion

    public SerialPortViewModel(SerialPortAdapter serialPortAdapter)
    {
        Communication = Serial = serialPortAdapter;
    }

    protected internal override void InitCommunication()
    {
        base.InitCommunication();
        ComPortList = Serial.GetPortNames();
        Serial.SerialOption.Parity = ParitiesSource.FirstOrDefault(it => it.Value == Parity.None)?.Value;
        Serial.SerialOption.StopBits = StopBitsSource.FirstOrDefault(it => it.Value == StopBits.One)?.Value;
        Serial.SerialOption.SerialPortName = ComPortList.FirstOrDefault();
        Serial.SerialOption.BaudRate = 9600;
        Serial.SerialOption.DataBits = 8;
    }


    protected override void HandleReceiveMessage(SerialPortMessage message, string strMessage)
    {
        if (Ui != null && ReceiveOption.DefaultWriteUi)
        {
            Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Receive");
            Ui.Logger.Message($"{strMessage}",
                "#2B2BFF");
        }
    }

    protected override void HandleSendMessage(string message)
    {
    }

    public override string ScriptType => "Script";
}