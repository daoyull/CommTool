using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using NetTool.Common;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public class NetLogger : RichTextBox, IUiLogger
{
    public static readonly DependencyProperty AutoScrollEndProperty = DependencyProperty.Register(
        nameof(AutoScrollEnd), typeof(bool), typeof(NetLogger), new PropertyMetadata(true));

    public bool AutoScrollEnd
    {
        get => (bool)GetValue(AutoScrollEndProperty);
        set => SetValue(AutoScrollEndProperty, value);
    }

    private Paragraph _paragraph;

    public NetLogger()
    {
        VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        Document = new FlowDocument();
        _paragraph = new Paragraph();
        _paragraph.Typography.Kerning = true;
        Document.LineHeight = 1;
        Document.Blocks.Add(_paragraph);
    }

    private Dictionary<string, SolidColorBrush> _colorDict = new();

    public void Message(string message, string color)
    {
        Dispatcher.Invoke(() =>
        {
            if (!_colorDict.TryGetValue(color, out var brush))
            {
                _colorDict[color] = BrushHelper.Parse(color);
                brush = _colorDict[color];
            }

            var run = new Run()
            {
                Text = message,
                Foreground = brush
            };
            _paragraph.Inlines.Add(run);
            if (AutoScrollEnd)
            {
                ScrollToEnd();
            }
        });
    }

    public void Info(string message)
    {
        Message(message, "#808080");
    }

    public void Success(string message)
    {
        Message(message, "#1BD66C");
    }

    public void Warning(string message)
    {
        Message(message, "#FFCE44");
    }

    public void Error(string message)
    {
        Message(message, "#E30519");
    }
}