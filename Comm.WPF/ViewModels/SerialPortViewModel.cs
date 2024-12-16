using System.IO.Ports;
using Common.Lib.Helpers;
using Common.Lib.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;
using Comm.WPF.Abstracts.Plugins;

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
    }

    protected internal override void InitCommunication()
    {
        base.InitCommunication();
        ComPortList = Serial.GetPortNames();
        Serial.SerialConnectOption.Parity = ParitiesSource.FirstOrDefault(it => it.Value == Parity.None)?.Value;
        Serial.SerialConnectOption.StopBits = StopBitsSource.FirstOrDefault(it => it.Value == StopBits.One)?.Value;
        Serial.SerialConnectOption.SerialPortName = ComPortList.FirstOrDefault();
        Serial.SerialConnectOption.BaudRate = 9600;
        Serial.SerialConnectOption.DataBits = 8;
    }


    protected override void HandleReceiveMessage(SerialMessage message, string strMessage)
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

    protected override void HandleSendMessage(byte[] bytes, string message)
    {
        var time = DateTime.Now;
        if (SendOption.DefaultWriteUi)
        {
            Ui.Logger.Info($"[{time:yyyy-MM-dd HH:mm:ss.fff}] Send");
            Ui.Logger.Primary(message);
        }
    }

    protected override void OnSendScript(byte[] buffer, string uiMessage)
    {
        var plugin = (SendScriptPlugin<SerialMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(SendScriptPlugin<SerialMessage>));
        plugin?.InvokeScript(engine =>
        {
            var array = engine.Script.arrayToUint8Array(buffer);
            engine.Script.send(array, DateTime.Now, uiMessage);
        });
    }

    protected override void OnReceiveScript(SerialMessage message, string uiMessage)
    {
        var plugin = (ReceiveScriptPlugin<SerialMessage>?)Plugins.FirstOrDefault(it =>
            it.GetType() == typeof(ReceiveScriptPlugin<SerialMessage>));
        plugin?.InvokeScript(engine =>
        {
            var array = engine.Script.arrayToUint8Array(message.Data);
            engine.Script.receive(array, message.Time, uiMessage);
        });
    }


    public override string ScriptType => "Serial";
}