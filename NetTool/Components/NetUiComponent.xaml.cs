﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Common.Lib.Ioc;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;
using NetTool.Module.Share;

namespace NetTool.Components;

public partial class NetDataComponent : INetUi
{
    public NetDataComponent()
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
        nameof(SendCommand), typeof(ICommand), typeof(NetDataComponent),
        new PropertyMetadata(default(ICommand)));

    public ICommand SendCommand
    {
        get => (ICommand)GetValue(SendCommandProperty);
        set => SetValue(SendCommandProperty, value);
    }

    #endregion

    #region 依赖属性

    public static readonly DependencyProperty ReceiveOptionProperty = DependencyProperty.Register(
        nameof(ReceiveOption), typeof(IReceiveOption), typeof(NetDataComponent),
        new PropertyMetadata(default(IReceiveOption)));

    public IReceiveOption ReceiveOption
    {
        get => (IReceiveOption)GetValue(ReceiveOptionProperty);
        set => SetValue(ReceiveOptionProperty, value);
    }


    public static readonly DependencyProperty SendOptionProperty = DependencyProperty.Register(
        nameof(SendOption), typeof(ISendOption), typeof(NetDataComponent),
        new PropertyMetadata(default(ISendOption)));


    public ISendOption SendOption
    {
        get => (ISendOption)GetValue(SendOptionProperty);
        set => SetValue(SendOptionProperty, value);
    }

    public static readonly DependencyProperty SendFrameProperty = DependencyProperty.Register(
        nameof(SendFrame), typeof(uint), typeof(NetDataComponent),
        new PropertyMetadata(default(uint)));


    public uint SendFrame
    {
        get => (uint)GetValue(SendFrameProperty);
        set => SetValue(SendFrameProperty, value);
    }

    public static readonly DependencyProperty ReceiveFrameProperty = DependencyProperty.Register(
        nameof(ReceiveFrame), typeof(uint), typeof(NetDataComponent), new PropertyMetadata(default(uint)));

    public uint ReceiveFrame
    {
        get => (uint)GetValue(ReceiveFrameProperty);
        set => SetValue(ReceiveFrameProperty, value);
    }

    public static readonly DependencyProperty SendBytesProperty = DependencyProperty.Register(
        nameof(SendBytes), typeof(uint), typeof(NetDataComponent), new PropertyMetadata(default(uint)));

    public uint SendBytes
    {
        get => (uint)GetValue(SendBytesProperty);
        set => SetValue(SendBytesProperty, value);
    }

    public static readonly DependencyProperty ReceiveBytesProperty = DependencyProperty.Register(
        nameof(ReceiveBytes), typeof(uint), typeof(NetDataComponent), new PropertyMetadata(default(uint)));

    public uint ReceiveBytes
    {
        get => (uint)GetValue(ReceiveBytesProperty);
        set => SetValue(ReceiveBytesProperty, value);
    }

    public INotify Notify { get; } = Ioc.Resolve<INotify>();


    public void AddSendFrame(uint add)
    {
        Dispatcher.BeginInvoke(() => { SendFrame += add; });
    }

    public void AddReceiveFrame(uint add)
    {
        Dispatcher.BeginInvoke(() => { ReceiveFrame += add; });
    }

    public void AddSendBytes(uint add)
    {
        Dispatcher.BeginInvoke(() => { SendBytes += add; });
    }

    public void AddReceiveBytes(uint add)
    {
        Dispatcher.BeginInvoke(() => { ReceiveBytes += add; });
    }

    #endregion

    public IGlobalOption GlobalOption => Ioc.Resolve<IGlobalOption>();
    public IUiLogger Logger { get; }

    public void ResetNumber()
    {
        SendFrame = ReceiveFrame = SendBytes = ReceiveBytes = 0;
    }

    public string ReceiveMessage => Dispatcher.Invoke(() => NetLogger.Text);

    public string SendMessage
    {
        get => Dispatcher.Invoke(() => SendTextBox.Text);
        set => Dispatcher.Invoke(() => SendTextBox.Text = value);
    }


    private void ResetNumberClick(object sender, RoutedEventArgs e)
    {
        ResetNumber();
    }

    private void ClearReceiveClick(object sender, RoutedEventArgs e)
    {
        Logger.ClearArea();
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