using Autofac;
using Common.Lib.Abstracts;
using Comm.Lib.Interface;
using Comm.Service.Components;
using Comm.Service.IO;
using Comm.Service.Service;

namespace Comm.Service;

public class CommServiceModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<SerialPortAdapter>();
        builder.RegisterType<TcpClientAdapter>();
        builder.RegisterType<TcpServerAdapter>();
        builder.RegisterType<UdpAdapter>();
        
        builder.RegisterType<StringBuilderPool>().SingleInstance();
        builder.RegisterType<ScriptManager>().As<IScriptManager>().SingleInstance();
    }
}