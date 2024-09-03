using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public partial class Notify : UserControl, INotify
{
    private const int MaxMessage = 5;

    record MessageQueue(string Message, SolidColorBrush Back, SolidColorBrush Fore);

    record MessageBorder(Border Border, TextBlock TextBlock);

    private List<MessageBorder> _list = new();

    public Notify()
    {
        InitializeComponent();
        InitNotifyBorder();
        Task.Run(Start);
    }

    private void InitNotifyBorder()
    {
        for (int i = 0; i < MaxMessage; i++)
        {
            var border = new Border();
            border.CornerRadius = new CornerRadius(8);
            border.Height = 40;
            border.Width = 200;
            border.Visibility = Visibility.Collapsed;

            var textBlock = new TextBlock();
            border.Child = textBlock;
            MainStackPanel.Children.Add(border);
            _list.Add(new MessageBorder(border, textBlock));
        }
    }

    private async Task? Start()
    {
        await foreach (var message in _channel.Reader.ReadAllAsync())
        {
            while (true)
            {
                var count = _list.Count(it => it.Border.Visibility == Visibility.Collapsed);
                if (count > 0)
                {
                    break;
                }

                await Task.Delay(50);
            }

            var messageBorder = _list.First(it => it.Border.Visibility == Visibility.Collapsed);
#pragma warning disable CS4014
            Task.Run(async () => { await ShowMessage(message, messageBorder); });
        }
    }

    private async Task ShowMessage(MessageQueue message, MessageBorder messageBorder)
    {
        // 设置信息
        messageBorder.Border.Background = message.Back;
        messageBorder.TextBlock.Text = message.Message;
        messageBorder.TextBlock.Foreground = message.Fore;

        // 开始的动画
        await StartAnimation(messageBorder.Border);
        // 显示200毫秒
        await Task.Delay(200);
        // 结束的动画
        await EndAnimation(messageBorder.Border);
        // 完成隐藏消息框
        messageBorder.Border.Visibility = Visibility.Hidden;
    }

    private const long Tick = 1000 / 60;

    private async Task StartAnimation(Border border)
    {
        
    }

    private async Task EndAnimation(Border border)
    {
    }


    private Channel<MessageQueue> _channel = Channel.CreateUnbounded<MessageQueue>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false
    });


    public async void Info(string message)
    {
        await _channel.Writer.WriteAsync(new MessageQueue(message, Brushes.Black, Brushes.White));
    }

    public async void Success(string message)
    {
    }

    public async void Warning(string message)
    {
    }

    public async void Error(string message)
    {
    }
}