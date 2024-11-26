using System.Windows;
using System.Windows.Controls;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using Microsoft.Win32;
using NetTool.ScriptManager.Interface;
using NetTool.ViewModels;

namespace NetTool.Views;

public partial class ScriptManagerView : Window
{
    public ScriptManagerView()
    {
        InitializeComponent();
        Loaded += HandleLoaded;
        Unloaded += HandleUnLoaded;
        this.DataContext = Ioc.Resolve<ScriptViewModel>();
    }

    public void Show(string type)
    {
        if (DataContext is ScriptViewModel viewModel)
        {
            viewModel.Type = type;
        }
        Show();
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