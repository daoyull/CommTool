using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using NetTool.Common;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public partial class Notify : INotify
{
    private const int MaxMessage = 5;

    record MessageQueue(string Message, SolidColorBrush Back, SolidColorBrush Fore);


    public Notify()
    {
        InitializeComponent();
        Task.Run(Start);
    }


    private int _index;

    private async Task? Start()
    {
        await foreach (var message in _channel.Reader.ReadAllAsync())
        {
            while (true)
            {
                if (_index < MaxMessage)
                {
                    _index++;
                    break;
                }

                await Task.Delay(50);
            }

            Dispatcher.InvokeAsync(async () =>
            {
                var border = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    MinHeight  = 40,
                    Width = 300,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = message.Back,
                    Padding = new Thickness(10, 3, 10, 3),
                    BorderBrush = Brushes.Gainsboro,
                    BorderThickness = new Thickness(1)
                };

                var textBlock = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = message.Fore,
                    Text = message.Message,
                    VerticalAlignment = VerticalAlignment.Center
                };

                border.Child = textBlock;
                MainStackPanel.Children.Add(border);

                DoubleAnimation animation1 = new DoubleAnimation();
                animation1.From = 0;
                animation1.To = GetTextWidth(message.Message, textBlock.FontFamily, textBlock.FontSize,
                    textBlock.FontStyle, textBlock.FontWeight);

                animation1.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                animation1.Completed += async (_, _) =>
                {
                    await Task.Delay(1000);
                    Storyboard storyboard = new Storyboard();
                    DoubleAnimation animation2 = new DoubleAnimation();
                    animation2.To = 0;
                    animation2.Duration = new Duration(TimeSpan.FromMilliseconds(250));
                    var animation3 = new DoubleAnimation();
                    animation3.From = border.ActualHeight;
                    animation3.To = 0;
                    animation2.Duration = new Duration(TimeSpan.FromMilliseconds(400));

                    storyboard.Children.Add(animation2);
                    storyboard.Children.Add(animation3);

                    Storyboard.SetTarget(animation2, border);
                    Storyboard.SetTargetProperty(animation2, new PropertyPath(nameof(Width)));
                    Storyboard.SetTarget(animation3, border);
                    Storyboard.SetTargetProperty(animation3, new PropertyPath(nameof(Height)));
                    storyboard.Begin();
                };
                border.BeginAnimation(Border.WidthProperty, animation1);
                await Task.Delay(1500);
                MainStackPanel.Children.Remove(border);
                _index--;
            });
        }
    }


    private double GetTextWidth(string text, FontFamily fontFamily, double fontSize, FontStyle fontStyle,
        FontWeight fontWeight)
    {
        // Create a FormattedText object to measure text width
        FormattedText formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(fontFamily, fontStyle, fontWeight, FontStretches.Normal),
            fontSize,
            Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

        return formattedText.Width + 40;
    }

    private Channel<MessageQueue> _channel = Channel.CreateUnbounded<MessageQueue>(new UnboundedChannelOptions
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false
    });


    public async void Info(string message)
    {
        await _channel.Writer.WriteAsync(new MessageQueue(message, BrushHelper.Parse("#EDF2FC"), BrushHelper.Parse("#909399")));
    }

    public async void Success(string message)
    {
        await _channel.Writer.WriteAsync(new MessageQueue(message, BrushHelper.Parse("#F0F9EB"), BrushHelper.Parse("#67C23A")));
    }

    public async void Warning(string message)
    {
        await _channel.Writer.WriteAsync(new MessageQueue(message, BrushHelper.Parse("#FDF6EC"), BrushHelper.Parse("#E6A23C")));
    }

    public async void Error(string message)
    {
        await _channel.Writer.WriteAsync(new MessageQueue(message, BrushHelper.Parse("#FEF0F0"), BrushHelper.Parse("#F56C6C")));
    }
}