

using Autofac;
using Common.Lib.Abstracts;
using NetTool.ScriptManager.Interface;

namespace NetTool.ScriptManager;

public class ScriptManagerModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<Service.ScriptManager>().As<IScriptManager>().SingleInstance();
    }
}