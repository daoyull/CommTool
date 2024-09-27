using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Lib.Ioc;
using NetTool.Lib.Interface;

namespace NetTool.Components;

public partial class CommunicationDataComponent : ICommunicationUi
{
    public CommunicationDataComponent()
    {
        InitializeComponent();
        Logger = NetLogger;
    }


    private async void OnSendTextKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                InsertNewLineAtCaret((TextBox)sender);
            }
            else
            {
                RaiseCommand();
            }
        }
    }

    public Func<string, bool>? CanInput { get; set; }

    private void RaiseCommand()
    {
        if (!string.IsNullOrEmpty(SendTextBox.Text) && SendCommand.CanExecute(SendTextBox.Text))
        {
            SendCommand.Execute(SendTextBox.Text);
        }
    }

    private void InsertNewLineAtCaret(TextBox textBox)
    {
        int caretIndex = textBox.CaretIndex;
        string text = textBox.Text;
        string newLine = Environment.NewLine; // 使用环境特定的新行字符
        textBox.Text = text.Insert(caretIndex, newLine);
        textBox.CaretIndex = caretIndex + newLine.Length; // 移动光标到新行的开始
    }

    private void OnSendTextPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var invoke = CanInput?.Invoke(e.Text);
        if (invoke == false)
        {
            e.Handled = true;
        }
    }

    #region Command

    public static readonly DependencyProperty SendCommandProperty = DependencyProperty.Register(
        nameof(SendCommand), typeof(ICommand), typeof(CommunicationDataComponent),
        new PropertyMetadata(default(ICommand)));

    public ICommand SendCommand
    {
        get => (ICommand)GetValue(SendCommandProperty);
        set => SetValue(SendCommandProperty, value);
    }

    #endregion

    #region 依赖属性

    public static readonly DependencyProperty SendFrameProperty = DependencyProperty.Register(
        nameof(SendFrame), typeof(uint), typeof(CommunicationDataComponent),
        new PropertyMetadata(default(uint)));


    public uint SendFrame
    {
        get => (uint)GetValue(SendFrameProperty);
        set => SetValue(SendFrameProperty, value);
    }

    public static readonly DependencyProperty ReceiveFrameProperty = DependencyProperty.Register(
        nameof(ReceiveFrame), typeof(uint), typeof(CommunicationDataComponent), new PropertyMetadata(default(uint)));

    public uint ReceiveFrame
    {
        get => (uint)GetValue(ReceiveFrameProperty);
        set => SetValue(ReceiveFrameProperty, value);
    }

    public static readonly DependencyProperty SendBytesProperty = DependencyProperty.Register(
        nameof(SendBytes), typeof(uint), typeof(CommunicationDataComponent), new PropertyMetadata(default(uint)));

    public uint SendBytes
    {
        get => (uint)GetValue(SendBytesProperty);
        set => SetValue(SendBytesProperty, value);
    }

    public static readonly DependencyProperty ReceiveBytesProperty = DependencyProperty.Register(
        nameof(ReceiveBytes), typeof(uint), typeof(CommunicationDataComponent), new PropertyMetadata(default(uint)));

    public uint ReceiveBytes
    {
        get => (uint)GetValue(ReceiveBytesProperty);
        set => SetValue(ReceiveBytesProperty, value);
    }

    public INotify Notify { get; } = Ioc.Resolve<INotify>();

    public void AddSendFrame(uint add)
    {
        Dispatcher.Invoke(() => { SendFrame += add; });
    }

    public void AddReceiveFrame(uint add)
    {
        Dispatcher.Invoke(() => { ReceiveFrame += add; });
    }

    public void AddSendBytes(uint add)
    {
        Dispatcher.Invoke(() => { SendBytes += add; });
    }

    public void AddReceiveBytes(uint add)
    {
        Dispatcher.Invoke(() => { ReceiveBytes += add; });
    }

    #endregion

    public IUiLogger Logger { get; }

    public void ResetNumber()
    {
        SendFrame = ReceiveFrame = SendBytes = ReceiveBytes = 0;
    }

    private void ResetNumberClick(object sender, RoutedEventArgs e)
    {
        ResetNumber();
    }

    private void ClearReceiveClick(object sender, RoutedEventArgs e)
    {
        Logger.ClearAllMessage();
    }

    private void ClearSendClick(object sender, RoutedEventArgs e)
    {
        SendTextBox.Text = "";
    }

    private void SendClick(object sender, RoutedEventArgs e)
    {
        RaiseCommand();
    }
}