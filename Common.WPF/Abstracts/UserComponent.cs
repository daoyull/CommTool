using System.Windows.Controls;
using Common.Lib.Service;
using Common.Mvvm.Abstracts;

namespace CommonWpf;

public  class UserComponent : UserControl, ILifeCycle, IPluginBuilder
{
    public HashSet<ILifePlugin> Plugins { get; } = new();

    public UserComponent()
    {
        Loaded += async (sender, args) => { await ((ILifeCycle)this).OnLoad(); };
        Unloaded += async (sender, args) => { await ((ILifeCycle)this).OnUnload(); };
        // ReSharper disable once VirtualMemberCallInConstructor
        OnCreated();
    }

    protected virtual void OnCreated()
    {
        ((ILifeCycle)this).OnCreate().GetAwaiter().GetResult();
    }

    public override async void BeginInit()
    {
        base.BeginInit();
      
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        await ((ILifeCycle)this).OnInit();
    }

    #region Builder

    public IPluginBuilder PluginBuilder => this;

    public IPluginBuilder AddPlugin<T>() where T : ILifePlugin
    {
        return AddPlugin((ILifePlugin)Activator.CreateInstance(typeof(T))!);
    }

    public IPluginBuilder AddPlugin(ILifePlugin plugin)
    {
        Plugins.Add(plugin);
        return this;
    }

    #endregion
}

/// <summary>
/// 用户组件
/// </summary>
public abstract class UserComponent<T> : UserComponent where T : BaseViewModel
{
    /// <summary>
    /// Autofac 获取ViewModel
    /// </summary>
    public T? ViewModel => (T?)DataContext;
    

    protected override void OnCreated()
    {
        PluginBuilder.AddPlugin<ViewModelPlugin<T>>();
        base.OnCreated();
    }
}