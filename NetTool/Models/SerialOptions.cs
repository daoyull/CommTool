using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;
using NetTool.Module.Options;

namespace NetTool.Models;

partial class SerialConnectOption : ObservableObject, ISerialConnectOption
{
    [ObservableProperty] private string? _serialPortName;
    [ObservableProperty] private int _baudRate;
    
    [ObservableProperty] private int _dataBits;

    [ObservableProperty] private StopBits? _stopBits;
    [ObservableProperty] private Parity? _parity;
    [ObservableProperty] private bool _canEditBaudRate;
}

partial class SerialReceiveOption : BaseReceiveOption, ISerialReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _autoNewLine;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    public bool AutoScroll { get; set; }
}

partial class SerialSendOption : BaseSendOption, ISerialSendOption;