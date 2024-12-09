using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTool.Module;

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

       
    }

    private void CreateDefaultServices(HostBuilderContext context, IServiceCollection service)
    {
        service.AddWpfBlazorWebView();
        service.AddBlazorWebViewDeveloperTools();

        Ioc.Register(builder =>
        {
            builder.RegisterModule<NetToolWpfModule>();
            builder.RegisterModule<NetToolModule>();
            builder.Populate(service);
        });
        Ioc.Builder();
    }
}