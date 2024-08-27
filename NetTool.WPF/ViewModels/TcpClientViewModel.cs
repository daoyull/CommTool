using System.IO;
using System.Text;
using Common.Mvvm.Abstracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetTool.Lib.Interface;
using NetTool.Module.Components;
using NetTool.WPF.Components;
using NetTool.WPF.Models;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace NetTool.WPF.ViewModels;

public partial class TcpClientViewModel : BaseViewModel, IDisposable
{
    [ObservableProperty] private TcpConfig _config = new();
    private TcpClient _client = new TcpClient();

    private IJavaScriptExec _scriptExec = new ScriptEngine();
    public IUiLogger? UiLogger { get; set; }

    [RelayCommand]
    private async Task Connection()
    {
        var verify = Config.Verify(out string errMsg);
        if (!verify)
        {
            return;
        }
        _scriptExec.Reload(await File.ReadAllTextAsync(@"D:\GitCode\NetTool\NetTool.Module\JavaScripts\DefaultScript.js"));
        _client.Received = (client, e) =>
        {
            //从服务器收到信息。但是一般byteBlock和requestInfo会根据适配器呈现不同的值。
            var mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
            UiLogger?.Info(mes);
            _scriptExec.OnReceived(e.ByteBlock.Buffer.Take(e.ByteBlock.Len).ToArray());
            return EasyTask.CompletedTask;
        };

        //载入配置
        await _client.SetupAsync(new TouchSocketConfig()
            .SetRemoteIPHost($"{Config.Ip}:{Config.Port}")
            .ConfigureContainer(a =>
            {
                a.AddConsoleLogger(); //添加一个日志注入
            }));

        await _client.ConnectAsync();
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}