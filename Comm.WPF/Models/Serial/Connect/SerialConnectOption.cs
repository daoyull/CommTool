using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using Comm.Lib.Interface;

namespace Comm.WPF.Models;

public partial class SerialConnectOption : BaseConnectOption, ISerialConnectOption
{
    [ObservableProperty] private string? _serialPortName;
    [ObservableProperty] private int _baudRate;

    [ObservableProperty] private int _dataBits;

    [ObservableProperty] private StopBits? _stopBits;
    [ObservableProperty] private Parity? _parity;
}