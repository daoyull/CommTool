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
    }

    private Channel<ReceiveArgs> _receiveChannel = Channel.CreateUnbounded<ReceiveArgs>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = true
    });
    

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

        var path = Path.Combine(Directory.GetCurrentDirectory(), "JavaScripts", "DefaultScript.js");

        _scriptExec.Reload(await File.ReadAllTextAsync(path));
        _client.Received += HandleReceive;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        Task.Run(StartHandleReceive);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

        //载入配置
        _client.Ip = Config.Ip;
        _client.Port = Config.Port;
        await _client.ConnectAsync();
    }

    private async Task? StartHandleReceive()
    {
        await foreach (var args in _receiveChannel.Reader.ReadAllAsync())
        {
            var mes = Encoding.UTF8.GetString(args.Buffer);
            UiLogger?.Info(mes + Environment.NewLine);
            _scriptExec.OnReceived(args.Buffer);
        }
    }

    private async void HandleReceive(object? sender, ReceiveArgs e)
    {
        await _receiveChannel.Writer.WriteAsync(e);
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}