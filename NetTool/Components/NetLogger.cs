using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using NetTool.Common;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public class NetLogger : TextEditor, IUiLogger
{
    public NetLogger()
    {
        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
        WordWrap = true;
        TextArea.TextView.LineTransformers.Add(_lineColorTransformer);
        Options.EnableEmailHyperlinks = false;
        Options.EnableTextDragDrop = false;
        Options.EnableHyperlinks = false;
        Options.EnableRectangularSelection = false;
        Options.EnableImeSupport = false;
    }

    readonly LineColorTransformer _lineColorTransformer = new LineColorTransformer();

    private void AppendLine(string message, string color)
    {
        AppendText(message + Environment.NewLine);
        _lineColorTransformer.AddLineColor(Document.LineCount - 1, color);
    }

    public void Message(string message, string color)
    {
        var lineList = message.Split(Environment.NewLine).ToList();
        Dispatcher.Invoke(() =>
        {
            foreach (var item in lineList)
            {
                AppendLine(item, color);
            }

            ScrollToEnd();
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

    public void ClearAllMessage()
    {
        _lineColorTransformer.Clear();
        Text = "";
    }
}

public class LineColorTransformer : DocumentColorizingTransformer
{
    public void Clear()
    {
        _lineColorDict.Clear();
    }

    private Dictionary<int, string> _lineColorDict = new();

    public void AddLineColor(int line, string color)
    {
        _lineColorDict[line] = color;
    }

    protected override void ColorizeLine(DocumentLine line)
    {
        if (_lineColorDict.TryGetValue(line.LineNumber, out var color) && !string.IsNullOrEmpty(color))
        {
            ChangeLinePart(line.Offset, line.EndOffset,
                sp => { sp.TextRunProperties.SetForegroundBrush(BrushHelper.Parse(color)); });
        }
    }
}