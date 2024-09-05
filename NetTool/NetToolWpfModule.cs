using Autofac;
using Common.Lib.Abstracts;
using NetTool.Module.Service;
using NetTool.Service;
using NetTool.ViewModels;

namespace NetTool;

public class NetToolWpfModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<TcpClientService>();
        builder.RegisterType<TcpClientViewModel>();
        builder.RegisterType<SettingService>().SingleInstance();
    }
}