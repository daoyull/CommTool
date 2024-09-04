using System.Windows;

namespace NetTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Notify.Info("提示消息");
        Notify.Success(("成功消息"));
        Notify.Warning("警告消息");
        Notify.Error("错误消息");
    }
}