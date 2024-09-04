using System.IO;
using System.Text;
using System.Threading.Channels;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.Lib.Args;
using NetTool.Lib.Interface;
using NetTool.Models;
using NetTool.Module.Components;
using NetTool.Module.Service;

namespace NetTool.ViewModels;

public partial class TcpClientViewModel : BaseViewModel, IDisposable
{
    public TcpClientViewModel(TcpClientService tcpClientService)
    {
        _client = tcpClientService;
        _client.Closed += HandleClosed;
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
    

    [ObservableProperty] private TcpConfig _config = new();
    private IJavaScriptExec _scriptExec = new ScriptEngine();
    private readonly TcpClientService _client;
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
        await _client.ConnectAsync();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "JavaScripts", "DefaultScript.js");

        _scriptExec.Reload(await File.ReadAllTextAsync(path));
        _client.Received += HandleReceive;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        Task.Run(StartHandleReceive);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

        //载入配置
        _client.Ip = Config.Ip;
        _client.Port = Config.Port!.Value;
        
        IsConnect = true;
    }

    private async Task? StartHandleReceive()
    {
        await foreach (var args in _receiveChannel.Reader.ReadAllAsync())
        {
            var mes = Encoding.UTF8.GetString(args.Buffer);
            UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Receive]{Environment.NewLine}");
            UiLogger?.Success($"{mes}{Environment.NewLine}{Environment.NewLine}");
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
        if (!_client.IsConnect || string.IsNullOrEmpty(SendString))
        {
            return;
        }
        await _client.SendAsync(Encoding.UTF8.GetBytes(SendString));
        UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send]{Environment.NewLine}");
        UiLogger?.Message($"{SendString}{Environment.NewLine}{Environment.NewLine}","#1E6FFF");
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}