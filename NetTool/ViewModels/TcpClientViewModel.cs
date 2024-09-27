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
using NetTool.Module.IO;
using NetTool.Module.Service;
using NetTool.Service;

namespace NetTool.ViewModels;

public partial class TcpClientViewModel : BaseViewModel, IDisposable
{
    private CancellationTokenSource? _showCts;

    public TcpClientViewModel(TcpClientAdapter tcpClient, SettingService settingService)
    {
        Client = tcpClient;
        _settingService = settingService;
        tcpClient.Connected += (sender, args) =>
        {
            IsConnect = Client.IsConnect;
            _showCts = new();
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Task.Run(StartHandleReceive, _showCts.Token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        };
        tcpClient.Closed += (sender, args) =>
        {
            IsConnect = Client.IsConnect;
            _showCts?.Cancel();
            _showCts?.Dispose();
            _showCts = null;
        };
    }

    [ObservableProperty] private bool _isConnect;
    public TcpClientAdapter Client { get; }
    private readonly SettingService _settingService;
    public IUiLogger? UiLogger { get; set; }

    [RelayCommand]
    private async Task Connection()
    {
        try
        {
            if (!IsConnect)
            {
                await Client.ConnectAsync();
            }
            else
            {
                Client.Close();
            }
        }
        catch (Exception e)
        {
            Client.Notify.Error(e.Message);
        }
    }

    private async Task? StartHandleReceive()
    {
        await foreach (var args in Client.MessageReadAsync())
        {
            string outMessage;
            if (Client.ReceiveOption.IsHex)
            {
                outMessage = args.Data.ToHexString();
            }
            else
            {
                outMessage = Encoding.UTF8.GetString(args.Data);
            }


            UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Receive]");
            UiLogger?.Success($"{outMessage}");
            if (Client.SendOption.IsEnableScript)
            {
                UiLogger?.Message(string.Empty, string.Empty);
            }
        }
    }

    [RelayCommand]
    private async Task Send(string message)
    {
        await SendAsync(message);
    }

    private async Task SendAsync(string message)
    {
        try
        {
            if (!Client.IsConnect || string.IsNullOrEmpty(message))
            {
                return;
            }

            byte[] sendBuffer;
            if (Client.SendOption.IsHex)
            {
                sendBuffer = message.HexStringToArray();
            }
            else
            {
                sendBuffer = Encoding.UTF8.GetBytes(message);
            }

            await Client.WriteAsync(sendBuffer, 0, sendBuffer.Length);

            string outMessage;
            if (Client.SendOption.IsHex)
            {
                outMessage = sendBuffer.ToHexString();
            }
            else
            {
                outMessage = Encoding.UTF8.GetString(sendBuffer);
            }

            UiLogger?.Info($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [Send]");
            UiLogger?.Message($"{outMessage}", "#1E6FFF");
            if (Client.SendOption.IsHex)
            {
                UiLogger?.Message(string.Empty, string.Empty);
            }

            if (Client.SendOption.AutoSend)
            {
                await Task.Delay(Client.SendOption.AutoSendTime);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    public void Dispose()
    {
        Client.Dispose();
    }
}


//switch (e.PropertyName)
//        {
//            case nameof(Models.SendOption.HexSend) :
//                if (SendOption.HexSend)
//                {
//                    SendString = _settingService.ToHexStr(SendString ?? "");
//                }
//                else
//{
//    SendString = _settingService.HexToStr(SendString ?? "");
//}

//break;
//            case nameof(Models.SendOption.ScheduleSend):
//    Task.Run(StartScheduleSend);
//    break;
//}