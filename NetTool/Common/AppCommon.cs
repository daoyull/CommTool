using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Lib.Ioc;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTool.Module;

namespace NetTool.Common;

public static class AppCommon
{
    public static void CreateAppCommon()
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

    private static void CreateDefaultServices(HostBuilderContext context, IServiceCollection service)
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

    public static void CreateMainWindowCommon()
    {
        PaletteHelper paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();
        theme.SetPrimaryColor(BrushHelper.Parse("#409EFF").Color);
        theme.SetSecondaryColor(BrushHelper.Parse("#D9ECFF").Color);
        paletteHelper.SetTheme(theme);
    }
}