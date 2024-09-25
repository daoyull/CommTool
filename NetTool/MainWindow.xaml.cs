using System.Windows;
using Common.Lib.Ioc;
using MaterialDesignThemes.Wpf;
using NetTool.Components;
using NetTool.Lib.Interface;

namespace NetTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NotifyGrid.Children.Add((Notify)Ioc.Resolve<INotify>());
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        
        Ioc.Resolve<INotify>().Info("提示消息");
        Ioc.Resolve<INotify>().Success(("成功消息"));
        Ioc.Resolve<INotify>().Warning("警告消息");
        Ioc.Resolve<INotify>().Error("错误消息");
        // DialogHost.Close("RootDialog");
        GC.Collect();
    }
}