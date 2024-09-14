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
        builder.RegisterType<TcpClientNet>();
        builder.RegisterType<TcpClientViewModel>();
        builder.RegisterType<SettingService>().SingleInstance();
    }
}