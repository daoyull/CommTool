using System.Configuration;
using System.Data;
using System.Windows;
using Autofac;
using Common.Lib.Ioc;
using NetTool.WPF;

namespace NetTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Ioc.Register(builder =>
        {
            builder.RegisterModule<NetToolWpfModule>();
        });
        Ioc.Builder();
    }
}