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

        builder.RegisterType<SerialConnectOption>().As<ISerialConnectOption>();
        builder.RegisterType<SerialReceiveOption>().As<ISerialReceiveOption>();
        builder.RegisterType<SerialSendOption>().As<ISerialSendOption>();

        builder.RegisterType<TcpClientConnectOption>().As<ITcpClientConnectOption>();
        builder.RegisterType<TcpClientReceiveOption>().As<ITcpClientReceiveOption>();
        builder.RegisterType<TcpClientSendOption>().As<ITcpClientSendOption>();

        builder.RegisterType<TcpServerConnectOption>().As<ITcpServerConnectOption>();
        builder.RegisterType<TcpServerReceiveOption>().As<ITcpServerReceiveOption>();
        builder.RegisterType<TcpServerSendOption>().As<ITcpServerSendOption>();

        builder.RegisterType<GlobalOption>().As<IGlobalOption>().SingleInstance();
        builder.RegisterType<Notify>().As<INotify>().SingleInstance();

        builder.RegisterType<ScriptManagerViewModel>().SingleInstance();
        builder.RegisterType<BlazorService>().SingleInstance();
    }
}