using System.Configuration;
using System.Data;
using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTool.Common;
using NetTool.Module;

namespace SerialPort;

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