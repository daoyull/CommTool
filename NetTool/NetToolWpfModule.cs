using Autofac;
using Common.Lib.Abstracts;
using NetTool.Components;
using NetTool.Lib.Interface;
using NetTool.Models;
using NetTool.Servcice;
using NetTool.ViewModels;

namespace NetTool;

public class NetToolWpfModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<TcpClientViewModel>();
        builder.RegisterType<TcpServerViewModel>();
        builder.RegisterType<SerialPortViewModel>();

        builder.RegisterType<SerialOption>().As<ISerialOption>();
        builder.RegisterType<SerialReceiveOption>().As<ISerialReceiveOption>();
        builder.RegisterType<SerialSendOption>().As<ISerialSendOption>();

        builder.RegisterType<TcpClientOption>().As<ITcpClientOption>();
        builder.RegisterType<TcpClientReceiveOption>().As<ITcpClientReceiveOption>();
        builder.RegisterType<TcpClientSendOption>().As<ITcpClientSendOption>();

        builder.RegisterType<TcpServerOption>().As<ITcpServerOption>();
        builder.RegisterType<TcpServerReceiveOption>().As<ITcpServerReceiveOption>();
        builder.RegisterType<TcpServerSendOption>().As<ITcpServerSendOption>();
        
        builder.RegisterType<GlobalOption>().As<IGlobalOption>().SingleInstance();
        builder.RegisterType<Notify>().As<INotify>().SingleInstance();

        builder.RegisterType<ScriptViewModel>().SingleInstance();
        builder.RegisterType<BlazorService>().SingleInstance();
    }
}