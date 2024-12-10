﻿using System.Windows;
using System.Windows.Controls;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using Microsoft.Win32;
using NetTool.Servcice;
using NetTool.ViewModels;

namespace NetTool.Views;

public partial class ScriptManagerView : Window
{
    private BlazorService BlazorService { get; } = Ioc.Resolve<BlazorService>();

    public ScriptManagerViewModel ScriptManagerViewModel { get; }

    public ScriptManagerView()
    {
        InitializeComponent();
        Loaded += HandleLoaded;
        Unloaded += HandleUnLoaded;
        this.DataContext = ScriptManagerViewModel = Ioc.Resolve<ScriptManagerViewModel>();
    }

    public void ShowDialog(string type, string initScriptContent)
    {
        if (DataContext is ScriptManagerViewModel viewModel)
        {
            viewModel.Type = type;
            viewModel.InitScriptContent = initScriptContent;
            viewModel.Refresh();
        }

        var showDialog = ShowDialog();
    }

    private async void HandleUnLoaded(object sender, RoutedEventArgs e)
    {
        BlazorService.Close();
        await BlazorWebView.DisposeAsync();
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        BlazorWebView.Services = new AutofacServiceProvider(Ioc.Container!);
        BlazorService.Start();
    }
}