using System.IO.Ports;
using Common.Lib.Helpers;
using Common.Lib.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.Service.Share;
using Comm.WPF.Abstracts;
using Comm.WPF.Common;
using Comm.WPF.Entity;
using Comm.WPF.Servcice;
using Comm.WPF.Servcice.V8;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.ViewModels;

public partial class SerialPortViewModel : AbstractCommViewModel<SerialMessage>
{
    public SerialPortAdapter Serial { get; }
    public override ICommunication<SerialMessage> Communication { get; }

    #region 数据源

    [ObservableProperty] private bool _canEditBaudRate;

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
    public List<EnumItem<StopBits>> StopBitsSource { get; } =
        EnumHelper.EnumConvertToList<StopBits>().Where(it => it.Name != nameof(StopBits.None)).ToList();

    /// <summary>
    /// 波特率
    /// </summary>
    public List<int> BaudRates { get; } = [1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000];

    /// <summary>
    /// 数据位
    /// </summary>
    public List<int> DataBits { get; } = [5, 6, 7, 8];

    #endregion

    public SerialPortViewModel(SerialPortAdapter serialPortAdapter)
    {
        Communication = Serial = serialPortAdapter;
        InitCommunication();
    }

    protected sealed override void InitCommunication()
    {
        base.InitCommunication();
        ComPortList = Serial.GetPortNames();
        Serial.SerialConnectOption.Parity = ParitiesSource.FirstOrDefault(it => it.Value == Parity.None)?.Value;
        Serial.SerialConnectOption.StopBits = StopBitsSource.FirstOrDefault(it => it.Value == StopBits.One)?.Value;
        Serial.SerialConnectOption.SerialPortName = ComPortList.FirstOrDefault();
        Serial.SerialConnectOption.BaudRate = 9600;
        Serial.SerialConnectOption.DataBits = 8;
        V8Receive.LoadEngine += engine => { engine.AddHostObject("serial", new JsSerial(this, engine)); };
        V8Send.LoadEngine += engine => { engine.AddHostObject("serial", new JsSerial(this, engine)); };
    }


    protected override void LogUiReceiveMessage(SerialMessage message)
    {
        if (ReceiveOption.LogStyleShow)
        {
            Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        }

        Ui.Logger.Success($"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }

    protected override void LogFileReceiveMessage(SerialMessage message)
    {
        FileLog.WriteMessage(Type, $"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        FileLog.WriteMessage(Type, $"{message.Data.BytesToString(ReceiveOption.IsHex)}");
    }


    [RelayCommand]
    private void RefreshSerialPort()
    {
        ComPortList = Serial.GetPortNames();
    }


    protected override void LogSendMessage(byte[] bytes)
    {
        var time = DateTime.Now;
        Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] Send");
        Ui.Logger.Primary(bytes.BytesToString(SendOption.IsHex));
    }

    protected override void LogFileSendMessage(byte[] buffer)
    {
        FileLog.WriteMessage(Type, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Receive");
        FileLog.WriteMessage(Type, $"{buffer.BytesToString(SendOption.IsHex)}");
    }

    protected override object InvokeSendScript(byte[] buffer)
    {
        var array = (ITypedArray<byte>)V8Send.Engine!.Invoke("arrayToUint8Array", buffer);
        var jsMessage = new JsMessage(array);
        return V8Send.Engine.Invoke("send", jsMessage);
    }

    protected override object InvokeReceiveScript(SerialMessage message)
    {
        var array = (ITypedArray<byte>)V8Receive.Engine!.Invoke("arrayToUint8Array", message.Data);
        var jsMessage = new JsMessage(array);
        return V8Receive.Engine.Invoke("receive", jsMessage);
    }


    protected override string Type => "Serial";
}