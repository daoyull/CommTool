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
    private readonly SerialPortAdapter _serialService;
    private readonly SettingService _settingService;

    public SerialPortViewModel(SerialPortAdapter serialPortAdapter, SettingService settingService, INotify notify)
    {
        _serialService = serialPortAdapter;
        _settingService = settingService;
        Notify = notify;
        ComPortList = serialPortAdapter.GetPortNames();
        SelectedParity = ParitiesSource.FirstOrDefault(it => it.Value == Parity.None)?.Value;
        SelectedStopBits = StopBitsSource.FirstOrDefault(it => it.Value == StopBits.One)?.Value;
        SelectedComPort = ComPortList.FirstOrDefault();
    }

    #region 串口属性

    [ObservableProperty] private List<string> _comPortList;
    [ObservableProperty] private string? _selectedComPort;

    // 校验位
    public List<EnumItem<Parity>> ParitiesSource { get; } = EnumHelper.EnumConvertToList<Parity>();
    [ObservableProperty] private Parity? _selectedParity;

    // 停止位
    public List<EnumItem<StopBits>> StopBitsSource { get; } = EnumHelper.EnumConvertToList<StopBits>();
    [ObservableProperty] private StopBits? _selectedStopBits;

    // 波特率
    public List<int> BaudRates { get; } = new()
    {
        9600, 19200, 38400, 57600, 115200
    };

    [ObservableProperty] private int? _baudRate = 9600;

    // 数据位
    public List<int> DataBits { get; } = new() { 5, 6, 7, 8 };
    public ICommunicationUi Ui { get; set; }
    public INotify Notify { get; set; }

    [ObservableProperty] private int _dataBit = 8;

    [ObservableProperty] private bool _isConnected;
    [ObservableProperty] private bool _receiveIsHex = true;
    [ObservableProperty] private bool _sendIsHex = true;

    #endregion


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
                _serialService.Dispose();
            }
            else
            {
                _cts = new();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(StartReceiveHandle, _cts.Token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                _serialService.PortName = SelectedComPort;
                _serialService.Parity = SelectedParity!.Value;
                _serialService.BaudRate = BaudRate!.Value;
                _serialService.DataBits = DataBit;
                _serialService.StopBits = SelectedStopBits!.Value;
                _serialService.Connect();
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
        await foreach (var message in _serialService.MessageReadAsync())
        {
            var buffer = message.Data;
            Ui.AddReceiveBytes((uint)buffer.Length);
            Ui.AddReceiveFrame(1);
            Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Receive");
            Ui.Logger.Message($" {(ReceiveIsHex ? buffer.ToHexString() : buffer.ToUtf8Str())}", "#2B2BFF");
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
            if (SendIsHex)
            {
                sendBuffer = sendStr.HexStringToArray();
            }
            else
            {
                sendBuffer = Encoding.UTF8.GetBytes(sendStr);
            }

            await _serialService.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            Ui.AddSendFrame(1);
            Ui.AddSendBytes((uint)sendBuffer.Length);
            Ui.Logger.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Send");
            Ui.Logger.Success($"{(SendIsHex ? sendBuffer.ToHexString() : sendBuffer.ToUtf8Str())}");
        }
        catch (Exception e)
        {
            Notify.Error(e.Message);
        }
    }
}