using Autofac;
using Common.Lib.Abstracts;
using NetTool.Components;
using NetTool.Lib.Interface;
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
        builder.RegisterType<SerialPortViewModel>();
        builder.RegisterType<SettingService>().SingleInstance();
        builder.RegisterType<Notify>().As<INotify>().SingleInstance();
    }
}