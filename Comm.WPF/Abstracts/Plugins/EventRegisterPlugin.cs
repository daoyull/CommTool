using Common.Lib.Service;
using Comm.Lib.Interface;

namespace Comm.WPF.Abstracts.Plugins;

internal class EventRegisterPlugin<T> : ILifePlugin where T : IMessage
{
    public async Task OnCreate(ILifeCycle lifeCycle)
    {
        if (lifeCycle is AbstractCommViewModel<T> viewModel)
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