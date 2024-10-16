using System.IO.Ports;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;

namespace NetTool.Models;

partial class SerialOption : ObservableObject, ISerialOption
{
    [ObservableProperty] private string? _serialPortName;
    [ObservableProperty] private int _baudRate;
    [ObservableProperty] private int _dataBits;

    [ObservableProperty] private StopBits? _stopBits;
    [ObservableProperty] private Parity? _parity;
}

partial class SerialReceiveOption : ObservableObject, ISerialReceiveOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _saveToFile;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _autoNewLine;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    public bool AutoScroll { get; set; }
}

partial class SerialSendOption : ObservableObject, ISerialSendOption
{
    [ObservableProperty] private bool _defaultWriteUi = true;
    [ObservableProperty] private bool _isHex;
    [ObservableProperty] private bool _isEnableScript;
    [ObservableProperty] private string? _scriptName;
    [ObservableProperty] private bool _autoSend;
    [ObservableProperty] private int _autoSendTime = 100;
}