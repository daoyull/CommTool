using Autofac;
using Common.Lib.Abstracts;
using NetTool.Components;
using NetTool.Lib.Interface;
using NetTool.Models;
using NetTool.Module.Service;
using NetTool.Service;
using NetTool.ViewModels;

namespace NetTool;

public class NetToolWpfModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<TcpClientViewModel>();
        builder.RegisterType<SerialPortViewModel>();

        builder.RegisterType<SerialOption>().As<ISerialOption>();
        builder.RegisterType<SerialReceiveOption>().As<ISerialReceiveOption>();
        builder.RegisterType<SerialSendOption>().As<ISerialSendOption>();

        builder.RegisterType<TcpClientOption>().As<ITcpClientOption>();
        builder.RegisterType<TcpClienReceiveOption>().As<ITcpClientReceiveOption>();
        builder.RegisterType<TcpClienSendOption>().As<ITcpClientSendOption>();

        builder.RegisterType<GlobalOption>().As<IGlobalOption>().SingleInstance();

        builder.RegisterType<SettingService>().SingleInstance();
        builder.RegisterType<Notify>().As<INotify>().SingleInstance();
    }
}