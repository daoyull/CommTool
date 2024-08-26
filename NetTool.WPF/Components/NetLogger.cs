using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using NetTool.Lib.Interface;
using NetTool.WPF.Common;

namespace NetTool.WPF.Components;

public class NetLogger : RichTextBox, IUiLogger
{
    private Paragraph _paragraph;

    public NetLogger()
    {
        _paragraph = new Paragraph();
        Document.Blocks.Add(_paragraph);

        IsReadOnly = true;
    }

    public void Message(string message, string color)
    {
        Dispatcher.Invoke(() =>
        {
            var run = new Run()
            {
                Text = message,
                Foreground = BrushHelper.Parse(color)
            };
            _paragraph.Inlines.Add(run);
        });
    }

    public void Info(string message)
    {
        Dispatcher.Invoke(() =>
        {
            var run = new Run
            {
                Text = message,
                Foreground = Brushes.Gray
            };
            _paragraph.Inlines.Add(run);
        });
    }

    public void Success(string message)
    {
        Dispatcher.Invoke(() =>
        {
            var run = new Run();
            run.Text = message;
            run.Foreground = Brushes.Green;
            _paragraph.Inlines.Add(run);
        });
    }

    public void Warning(string message)
    {
        Dispatcher.Invoke(() =>
        {
            var run = new Run
            {
                Text = message,
                Foreground = Brushes.Orange
            };
            _paragraph.Inlines.Add(run);
        });
    }

    public void Error(string message)
    {
        Dispatcher.Invoke(() =>
        {
            var run = new Run
            {
                Text = message,
                Foreground = Brushes.Red
            };
            _paragraph.Inlines.Add(run);
        });
    }
}