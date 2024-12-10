using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using LanguageExt;
using NetTool.Common;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public class NetLogger : TextEditor, IUiLogger
{
    private CancellationTokenSource _cts = new();

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
        IsReadOnly = true;
        ICSharpCode.AvalonEdit.Search.SearchPanel.Install(this);
        this.PreviewMouseWheel += HandlePreviewMouseWheel;
        Unloaded += (sender, args) => { _cts.Dispose(); };

        Task.Run(StartWriteMessageToUi, _cts.Token);
    }

    private async Task? StartWriteMessageToUi()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(Tick);
                    bool canInvoke = false;
                    while (_writeQueue.Count > 0)
                    {
                        var item = _writeQueue.Dequeue();
                        if (string.IsNullOrEmpty(item.Item1))
                        {
                            item.Item1 = "";
                        }

                        var lineList = item.Item1.Split(Environment.NewLine).ToList();
                        Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var message in lineList)
                            {
                                AppendLine(message, item.Item2);
                            }
                        });
                        canInvoke = true;
                    }

                    if (canInvoke)
                    {
                        TickUpdate.Invoke();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        catch (OperationCanceledException e)
        {
            // ignore
        }
    }


    private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
            e.Handled = true;
            if (e.Delta > 0)
            {
                FontSize += 0.2;
            }
            else if (e.Delta < 0)
            {
                FontSize -= 0.2;
            }
        }
    }


    /// <summary>
    /// 每秒更新60次
    /// </summary>
    private const int Tick = 1000 / 60;

    readonly LineColorTransformer _lineColorTransformer = new();

    private bool isChange = false;

    private void AppendLine(string message, string color)
    {
        if (message.Length > 2048)
        {
            foreach (var item in SplitStringIntoChunks(message, 2048))
            {
                AppendText(item);
            }
        }
        else
        {
            AppendText(message);
        }


        if (!message.EndsWith(Environment.NewLine))
        {
            AppendText(Environment.NewLine);
        }

        _lineColorTransformer.AddLineColor(Document.LineCount - 1, color);
    }


    public void Write(string message, string color)
    {
        _writeQueue.Enqueue((message, color));
    }

    private Queue<(string, string)> _writeQueue = new();

    public void Info(string message)
    {
        Write(message, "#808080");
    }

    public void Primary(string message)
    {
        Write(message, "#2B2BFF");
    }

    public void Success(string message)
    {
        Write(message, "#1BD66C");
    }

    public void Warning(string message)
    {
        Write(message, "#FFCE44");
    }

    public void Error(string message)
    {
        Write(message, "#E30519");
    }

    public void ClearArea()
    {
        _lineColorTransformer.Clear();
        Text = "";
    }

    public Action TickUpdate { get; set; }

    private string[] SplitStringIntoChunks(string input, int chunkSize)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= chunkSize)
        {
            return new string[] { input };
        }

        List<string> chunks = new List<string>();

        for (int i = 0; i < input.Length; i += chunkSize)
        {
            // 如果剩余部分小于chunkSize，则取剩余的所有字符
            if (i + chunkSize > input.Length)
            {
                chunks.Add(input.Substring(i));
            }
            else
            {
                chunks.Add(input.Substring(i, chunkSize));
            }
        }

        return chunks.ToArray();
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