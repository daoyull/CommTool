using Autofac;
using Common.Lib.Abstracts;
using NetTool.Module.IO;

namespace NetTool.Module;

public class NetToolModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<SerialPortAdapter>();
        builder.RegisterType<TcpClientAdapter>();
    }
}