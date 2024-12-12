﻿using System.Windows;
using Comm.Lib.Interface;
using Common.Lib.Ioc;
using Comm.WPF.Common;
using Comm.WPF.Components;


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