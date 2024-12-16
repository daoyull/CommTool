using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Servcice;

public class JsUdp
{
    public UdpViewModel ViewModel { get; }

    public JsUdp(UdpViewModel viewModel)
    {
        ViewModel = viewModel;
    }
    
    public void sendBuffer(string address, byte[] buffer, int offset, int size)
    {
        
    }

    public void sendBuffer(string address, byte[] buffer) => sendBuffer(address, buffer, 0, buffer.Length);

    public void sendBuffer(string address, ITypedArray<byte> array, int offset, int size)
    {
        var buffer = array.ToArray();
        sendBuffer(address, buffer, offset, size);
    }

    public void sendBuffer(string address, ITypedArray<byte> array)
    {
        var buffer = array.ToArray();
        sendBuffer(address, buffer, 0, buffer.Length);
    }


    public void send(string address, string message)
    {
        var buffer = ViewModel.StringToBuffer(message);
        sendBuffer(address, buffer, 0, buffer.Length);
    }
    
    
}