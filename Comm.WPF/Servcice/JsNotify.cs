using Comm.Lib.Interface;
using Comm.WPF.Abstracts;

namespace Comm.WPF.Servcice;

public class JsNotify<T> where T : IMessage
{
    public AbstractCommViewModel<T> ViewModel { get; }


    public JsNotify(AbstractCommViewModel<T> viewModel)
    {
        ViewModel = viewModel;
    }

    public void info(string message)
    {
        ViewModel.Ui.Notify.Info(message);
    }
    
    public void success(string message)
    {
        ViewModel.Ui.Notify.Success(message);
    }
    
    public void waring(string message)
    {
        ViewModel.Ui.Notify.Warning(message);
    }
    
    public void error(string message)
    {
        ViewModel.Ui.Notify.Info(message);
    }
}