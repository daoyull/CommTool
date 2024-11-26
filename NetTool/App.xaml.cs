using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTool.Module;
using NetTool.ScriptManager;

namespace NetTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
#if DEBUG
        hostBuilder.UseEnvironment("Development");
#else
        hostBuilder.UseEnvironment("Production");
#endif

        var builder = hostBuilder.ConfigureServices(CreateDefaultServices);
        builder.ConfigureLogging((context, loggingBuilder) => { loggingBuilder.AddConsole(); });

        using IHost host = builder
            .Build();

        host.Start();

        PaletteHelper paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetPrimaryColor(Colors.Aqua);
    }

    private void CreateDefaultServices(HostBuilderContext context, IServiceCollection service)
    {
        service.AddWpfBlazorWebView();
        service.AddBlazorWebViewDeveloperTools();

        Ioc.Register(builder =>
        {
            builder.RegisterModule<NetToolWpfModule>();
            builder.RegisterModule<NetToolModule>();
            builder.RegisterModule<ScriptManagerModule>();
            builder.Populate(service);
        });
        Ioc.Builder();
    }
}