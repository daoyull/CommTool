using System.Windows;
using Common.Lib.Ioc;
using NetTool.Common;
using NetTool.Components;
using NetTool.Lib.Interface;

namespace SerialPort;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NotifyGrid.Children.Add((Notify)Ioc.Resolve<INotify>());
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AppCommon.CreateMainWindowCommon();
    }
}