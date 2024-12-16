using Comm.Lib.Interface;
using Comm.WPF.Abstracts;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Servcice;

public class JsComm<T> where T : IMessage
{
    public AbstractCommViewModel<T> ViewModel { get; }


    public JsComm(AbstractCommViewModel<T> viewModel)
    {
        ViewModel = viewModel;
    }

    public void sendBuffer(byte[] buffer, int offset, int size) => ViewModel.Communication.Write(buffer, offset, size);
    
    public void sendBuffer(byte[] buffer) => ViewModel.Communication.Write(buffer, 0, buffer.Length);

    public void sendBuffer(ITypedArray<byte> array, int offset, int size)
    {
        var buffer = array.ToArray();
        sendBuffer(buffer, offset, size);
    }

    public void sendBuffer(ITypedArray<byte> array)
    {
        var buffer = array.ToArray();
        sendBuffer(buffer, 0, buffer.Length);
    }


    public void send(string message)
    {
        var buffer = ViewModel.GetSendBuffer(message);
        sendBuffer(buffer, 0, buffer.Length);
    }
}