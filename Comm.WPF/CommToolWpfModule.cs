using Autofac;
using Common.Lib.Abstracts;
using Comm.Lib.Interface;
using Comm.WPF.Components;
using Comm.WPF.Models;
using Comm.WPF.Servcice;
using Comm.WPF.ViewModels;
using Comm.WPF.Views;

namespace Comm.WPF;

public class CommToolWpfModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<TcpClientViewModel>();
        builder.RegisterType<TcpServerViewModel>();
        builder.RegisterType<UdpViewModel>();
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


        builder.RegisterType<UdpConnectOption>().As<IUdpConnectOption>();
        builder.RegisterType<UdpReceiveOption>().As<IUdpReceiveOption>();
        builder.RegisterType<UdpSendOption>().As<IUdpSendOption>();

        builder.RegisterType<GlobalOption>().As<IGlobalOption>().SingleInstance();
        builder.RegisterType<Notify>().As<INotify>().SingleInstance();

        builder.RegisterType<ScriptManagerViewModel>().SingleInstance();
        builder.RegisterType<BlazorService>().SingleInstance();
    }
}