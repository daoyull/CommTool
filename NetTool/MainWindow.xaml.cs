using System.Windows;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using MaterialDesignThemes.Wpf;
using NetTool.Components;
using NetTool.Lib.Interface;
using NetTool.Views;

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
        var scriptManagerView = new ScriptManagerView();
        scriptManagerView.Show();

        GC.Collect();
    }
}