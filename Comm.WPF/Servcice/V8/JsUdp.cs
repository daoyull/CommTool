using Comm.Service.Share;
using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Servcice.V8;

public class JsUdp
{
    public UdpViewModel ViewModel { get; }

    public JsUdp(UdpViewModel viewModel)
    {
        ViewModel = viewModel;
    }


    public void sendBuffer(string address, byte[] buffer)
    {
    }

    public void sendBuffer(string address, ITypedArray<byte> array)
    {
        var buffer = array.ToArray();
        sendBuffer(address, buffer);
    }


    public void send(string address, string message, bool isHexStr = false)
    {
        var buffer = message.StringToBytes(isHexStr);
        sendBuffer(address, buffer);
    }
}