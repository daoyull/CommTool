using Autofac;
using Common.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Components;
using NetTool.Module.IO;
using NetTool.Module.Service;

namespace NetTool.Module;

public class NetToolModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<SerialPortAdapter>();
        builder.RegisterType<TcpClientAdapter>();
        builder.RegisterType<TcpServerAdapter>();
        builder.RegisterType<StringBuilderPool>().SingleInstance();
        builder.RegisterType<ScriptManager>().As<IScriptManager>().SingleInstance();
    }
}