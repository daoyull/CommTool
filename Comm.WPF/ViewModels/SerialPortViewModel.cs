using System.IO.Ports;
using Common.Lib.Helpers;
using Common.Lib.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;
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


    protected override void LogReceiveMessage(SerialMessage message, string strMessage)
    {
        if (ReceiveOption.LogStyleShow)
        {
            Ui.Logger.Info($"[{message.Time:yyyy-MM-dd HH:mm:ss.fff}] Receive");
            Ui.Logger.Success($"{strMessage}");
        }
        else
        {
            Ui.Logger.Success($"{strMessage}");
        }
    }


    [RelayCommand]
    private void RefreshSerialPort()
    {
        ComPortList = Serial.GetPortNames();
    }
    

    protected override void LogSendMessage(byte[] bytes, string message)
    {
        var time = DateTime.Now;
        if (SendOption.DefaultWriteUi)
        {
            Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] Send");
            Ui.Logger.Primary(message);
        }
    }

    protected override void InvokeSendScript(byte[] buffer, string uiMessage)
    {
    }

    protected override object InvokeReceiveScript(SerialMessage message)
    {
        var array = (ITypedArray<byte>)V8Receive.Engine!.Invoke("arrayToUint8Array", message.Data);
        var jsMessage = new JsMessage(array);
        return V8Receive.Engine.Invoke("receive", jsMessage);
    }


    protected override string ScriptType => "Serial";
}