using Comm.Lib.Interface;
using Comm.WPF.Abstracts;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Servcice.V8;

public class JsComm<T> where T : IMessage
{
    public AbstractCommViewModel<T> ViewModel { get; }


    public JsComm(AbstractCommViewModel<T> viewModel)
    {
        ViewModel = viewModel;
    }


    public async void send(string message)
    {
        await ViewModel.SendMessage(message);
    }
}