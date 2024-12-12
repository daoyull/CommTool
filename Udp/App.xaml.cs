﻿using System.Configuration;
using System.Data;
using System.Windows;
using Comm.WPF.Common;

namespace Udp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        AppCommon.CreateAppCommon();
    }
}