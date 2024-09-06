using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Channels;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Models;
using NetTool.Module.Common;
using NetTool.Module.Components;
using NetTool.Module.Service;
using NetTool.Service;

namespace NetTool.ViewModels;

public partial class TcpClientViewModel : BaseViewModel, IDisposable
{
    public TcpClientViewModel(TcpClientService tcpClientService, SettingService settingService)
    {
        _client = tcpClientService;
        _settingService = settingService;
        _client.Closed += HandleClosed;
        ReceiveOption.PropertyChanged += HandleReceiveOptionChanged;
        SendOption.PropertyChanged += HandleSendOptionChanged;
    }

    private void HandleSendOptionChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Models.SendOption.HexSend):
                if (SendOption.HexSend)
                {
                    SendString = _settingService.ToHexStr(SendString ?? "");
                }
                else
                {
                    SendString = _settingService.HexToStr(SendString ?? "");
                }

                break;
            case nameof(Models.SendOption.ScheduleSend):
                Task.Run(StartScheduleSend);
                break;
        }
    }

    private async Task StartScheduleSend()
    {
        while (SendOption.ScheduleSend)
        {
            await SendAsync();
            await Task.Delay(SendOption.ScheduleSendTime);
        }
    }

    private void HandleReceiveOptionChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Models.ReceiveOption.AutoBreak):
                _client.AutoBreak = ReceiveOption.AutoBreak;
                break;
            case nameof(Models.ReceiveOption.AutoBreakTime):
                _client.AutoBreakTime = ReceiveOption.AutoBreakTime;
                break;
        }
    }

    private void HandleClosed(object? sender, ClosedArgs e)
    {
        IsConnect = false;
        _client.Received -= HandleReceive;
    }

    private Channel<ReceiveArgs> _receiveChannel = Channel.CreateUnbounded<ReceiveArgs>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = true
    });

    [ObservableProperty] private string? _sendString;
    [ObservableProperty] private bool _isConnect;

    [ObservableProperty] private ReceiveOption _receiveOption = new();
    [ObservableProperty] private SendOption _sendOption = new();


    [ObservableProperty] private TcpConfig _config = new();
    private IJavaScriptExec _scriptExec = new ScriptEngine();
    private readonly TcpClientService _client;
    private readonly SettingService _settingService;
    public IUiLogger? UiLogger { get; set; }

    public bool CanConnect()
    {
        return !_client.IsConnect;
    }

    [RelayCommand(CanExecute = nameof(CanConnect))]
    private async Task Connection()
    {
        var verify = Config.Verify(out string errMsg);
        if (!verify)
        {
            return;
        }

        //载入配置
        _client.Ip = Config.Ip;
        _client.Port = Config.Port!.Value;
        _client.Received += HandleReceive;
        await _client.ConnectAsync();
        IsConnect = true;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        Task.Run(StartHandleReceive);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        var path = Path.Combine(Directory.GetCurrentDirectory(), "JavaScripts", "DefaultScript.js");
        _scriptExec.Reload(await File.ReadAllTextAsync(path));
    }

    private async Task? StartHandleReceive()
    {
        await foreach (var args in _receiveChannel.Reader.ReadAllAsync())
        {
            string outMessage;
            if (ReceiveOption.HexDisplay)
            {
                outMessage = args.Buffer.ToHexString();
            }
            else
            {
                outMessage = Encoding.UTF8.GetString(args.Buffer);
            }


            UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Receive]");
            UiLogger?.Success($"{outMessage}");
            if (ReceiveOption.AutoNewLine)
            {
                UiLogger?.Message(string.Empty,string.Empty);
            }

            _scriptExec.OnReceived(args.Buffer);
        }
    }

    private async void HandleReceive(object? sender, ReceiveArgs e)
    {
        await _receiveChannel.Writer.WriteAsync(e);
    }

    [RelayCommand]
    private async Task Send()
    {
        await SendAsync();
    }

    private async Task SendAsync()
    {
        try
        {
            if (!_client.IsConnect || string.IsNullOrEmpty(SendString))
            {
                return;
            }

            byte[] sendBuffer;
            if (SendOption.HexSend)
            {
                sendBuffer = SendString.HexStringToArray();
            }
            else
            {
                sendBuffer = Encoding.UTF8.GetBytes(SendString);
            }

            await _client.SendAsync(sendBuffer);

            string outMessage;
            if (SendOption.HexSend)
            {
                outMessage = sendBuffer.ToHexString();
            }
            else
            {
                outMessage = Encoding.UTF8.GetString(sendBuffer);
            }
            
            UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
            UiLogger?.Message($"{outMessage}", "#1E6FFF");
            if (ReceiveOption.AutoNewLine)
            {
                UiLogger?.Message(string.Empty, string.Empty);
            }
            if (SendOption.ScheduleSend)
            {
                await Task.Delay(SendOption.ScheduleSendTime);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}