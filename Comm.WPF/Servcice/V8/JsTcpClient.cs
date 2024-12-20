using Comm.Service.Share;
using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Servcice.V8;

public class JsTcpClient
{
    public TcpClientViewModel ViewModel { get; }

    public JsTcpClient(TcpClientViewModel viewModel)
    {
        ViewModel = viewModel;
    }


    public void sendBuffer(byte[] buffer) => ViewModel.Communication.Write(buffer, 0, buffer.Length);


    public void sendBuffer(ITypedArray<byte> array)
    {
        var buffer = array.ToArray();
        sendBuffer(buffer);
    }


    public void send(string message, bool isHexStr = false)
    {
        var buffer = message.StringToBytes(isHexStr);
        sendBuffer(buffer);
    }
}