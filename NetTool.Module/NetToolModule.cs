using Autofac;
using Common.Lib.Abstracts;
using NetTool.Module.Components;
using NetTool.Module.IO;

namespace NetTool.Module;

public class NetToolModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<SerialPortAdapter>();
        builder.RegisterType<TcpClientAdapter>();
        builder.RegisterType<StringBuilderPool>().SingleInstance();
    }
}