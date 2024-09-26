using System.Windows;
using System.Windows.Controls;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using Microsoft.Win32;

namespace NetTool.Views;

public partial class ScriptManagerView : Window
{
    public ScriptManagerView()
    {
        InitializeComponent();
        Loaded += HandleLoaded;
        Unloaded += HandleUnLoaded;
    }

    private async void HandleUnLoaded(object sender, RoutedEventArgs e)
    {
        await BlazorWebView.DisposeAsync();
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        BlazorWebView.Services = new AutofacServiceProvider(Ioc.Container!);
    }
}