using Autofac;
using Common.Lib.Abstracts;
using NetTool.WPF.ViewModels;

namespace NetTool.WPF;

public class NetToolWpfModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<TcpClientViewModel>();
    }
}