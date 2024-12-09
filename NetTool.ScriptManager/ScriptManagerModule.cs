

using Autofac;
using Common.Lib.Abstracts;
using NetTool.ScriptManager.Interface;
using NetTool.ScriptManager.Service;

namespace NetTool.ScriptManager;

public class ScriptManagerModule : BaseModule
{
    public override void LoadService(ContainerBuilder builder)
    {
        builder.RegisterType<Service.ScriptManager>().As<IScriptManager>().SingleInstance();
        builder.RegisterType<ScriptEngine>();
    }
}