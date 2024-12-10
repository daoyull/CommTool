using Common.Lib.Service;
using NetTool.Lib.Interface;

namespace NetTool.Abstracts.Plugins;

internal class EventRegisterPlugin<T> : ILifePlugin where T : IMessage
{
    public async Task OnCreate(ILifeCycle lifeCycle)
    {
        if (lifeCycle is AbstractNetViewModel<T> viewModel)
        {
            viewModel.InitCommunication();
        }
    }

    public Task OnInit(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnLoad(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }

    public Task OnUnload(ILifeCycle lifeCycle)
    {
        return Task.CompletedTask;
    }
}