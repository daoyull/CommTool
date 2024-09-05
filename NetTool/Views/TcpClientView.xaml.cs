using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NetTool.Views;

public partial class TcpClientView
{
    public TcpClientView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        if (ViewModel != null)
        {
            ViewModel.UiLogger = NetLogger;
        }
    }


    private async void OnSendTextKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                InsertNewLineAtCaret((TextBox)sender);
            }
            else
            {
                await ViewModel?.SendCommand.ExecuteAsync(null)!;
            }
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
        if (sender is not TextBox box)
        {
            return;
        }
        if (ViewModel!.SendOption.HexSend && !"0123456789ABCDEFabcdef".Contains(e.Text))
        {
            e.Handled = true;
            // todo 提示
        }
    }
}