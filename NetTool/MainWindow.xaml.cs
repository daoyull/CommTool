using System.Windows;
using System.Windows.Media;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using MaterialDesignThemes.Wpf;
using NetTool.Common;
using NetTool.Components;
using NetTool.Lib.Interface;
using NetTool.ViewModels;
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
        ContentControl.Content = new SerialPortView();
        NotifyGrid.Children.Add((Notify)Ioc.Resolve<INotify>());
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        PaletteHelper paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetPrimaryColor(BrushHelper.Parse("#A0CFFF").Color);
        paletteHelper.SetTheme(theme);
      
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var scriptManagerView = new ScriptManagerView();
        scriptManagerView.Show("SerialReceive", @"yuzhijiaoben");
    }
}