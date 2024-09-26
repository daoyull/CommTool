using System.IO.Ports;
using System.Text;
using Common.Lib.Helpers;
using Common.Lib.Models;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.Lib.Interface;
using NetTool.Module.Common;
using NetTool.Module.IO;
using NetTool.Service;

namespace NetTool.ViewModels;

public partial class SerialPortViewModel : BaseViewModel
{
    private readonly SettingService _settingService;
    public SerialPortAdapter Serial { get; }
    public ICommunicationUi Ui { get; set; } = null!;
    private INotify Notify { get; }
    public ISerialReceiveOption ReceiveOption { get; }
    public ISerialSendOption SendOption { get; }

    [ObservableProperty] private bool _isConnected;

    public SerialPortViewModel(SerialPortAdapter serialPortAdapter, SettingService settingService, INotify notify,
        ISerialReceiveOption receiveOption, ISerialSendOption sendOption)
    {
        _settingService = settingService;
        Serial = serialPortAdapter;
        Notify = notify;
        ReceiveOption = receiveOption;
        SendOption = sendOption;
        InitDefaultValue();
    }

    private void InitDefaultValue()
    {
        ComPortList = Serial.GetPortNames();
        Serial.SerialOption.Parity = ParitiesSource.FirstOrDefault(it => it.Value == Parity.None)?.Value;
        Serial.SerialOption.StopBits = StopBitsSource.FirstOrDefault(it => it.Value == StopBits.One)?.Value;
        Serial.SerialOption.SerialPortName = ComPortList.FirstOrDefault();
        Serial.SerialOption.BaudRate = 9600;
        Serial.SerialOption.DataBits = 8;
    }


    private CancellationTokenSource? _cts;

    [RelayCommand]
    private async Task Connection()
    {
        try
        {
            if (IsConnected)
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
                Serial.Close();
                Serial.Dispose();
                Notify.Info("已关闭连接");
            }
            else
            {
                _cts = new();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(StartReceiveHandle, _cts.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
    }

    private async Task StartReceiveHandle()
    {
        await foreach (var message in Serial.MessageReadAsync())
        {
            var buffer = message.Data;
            Ui.AddReceiveBytes((uint)buffer.Length);
            Ui.AddReceiveFrame(1);
            if (ReceiveOption.DefaultWriteUi)
            {
                Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Receive");
                Ui.Logger.Message($" {(ReceiveOption.IsHex ? buffer.ToHexString() : buffer.ToUtf8Str())}", "#2B2BFF");
            }
        }
    }


    [RelayCommand]
    private async Task Send(string sendStr)
    {
        try
        {
            if (!IsConnected || string.IsNullOrEmpty(sendStr))
            {
                Notify.Warning("串口未连接或发送内容为空");
                return;
            }

            byte[] sendBuffer;
            if (SendOption.IsHex)
            {
                sendBuffer = sendStr.HexStringToArray();
            }
            else
            {
                sendBuffer = Encoding.UTF8.GetBytes(sendStr);
            }

            await Serial.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            Ui.AddSendFrame(1);
            Ui.AddSendBytes((uint)sendBuffer.Length);
            if (SendOption.DefaultWriteUi)
            {
                Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Send");
                Ui.Logger.Success($"{(SendOption.IsHex ? sendBuffer.ToHexString() : sendBuffer.ToUtf8Str())}");
            }
        }
        catch (Exception e)
        {
            Notify.Error(e.Message);
        }
    }
}

public partial class SerialPortViewModel
{
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
}