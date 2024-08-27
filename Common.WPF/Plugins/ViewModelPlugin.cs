using Autofac;
using Common.Lib.Ioc;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace CommonWpf;

internal class ViewModelPlugin<T> : ILifePlugin where T : BaseViewModel
{
    public async Task OnCreate(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return;
        try
        {
            // Ioc加载ViewModel
            // 设计期加载异常
            var viewModel = Ioc.IsBuilder ? Ioc.Container?.ResolveOptional<T>() : default;
            userComponent.DataContext = viewModel;
            await ((ILifeCycle)viewModel!).OnCreate();
        }
        catch (Exception e)
        {
            // Ignore
            Console.WriteLine(e.Message);
        }
    }

    public async Task OnInit(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return;
        await ((ILifeCycle)userComponent.ViewModel!).OnInit();
    }

    public async Task OnLoad(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return;
        await ((ILifeCycle)userComponent.ViewModel!).OnLoad();
    }

    public async Task OnUnload(ILifeCycle lifeCycle)
    {
        if (lifeCycle is not UserComponent<T> userComponent) return;
        await ((ILifeCycle)userComponent.ViewModel!).OnUnload();
    }
    
    
}