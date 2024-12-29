using Comm.Service.Share;
using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Servcice.V8;

public class JsTcpServer
{
    public TcpServerViewModel ViewModel { get; }

    public JsTcpServer(TcpServerViewModel viewModel, V8ScriptEngine engine)
    {
        ViewModel = viewModel;
    }

    public void sendBuffer(string address, byte[] buffer)
    {
        var clientItem = ViewModel.Clients.FirstOrDefault(it => it.Socket.ToRemoteIpStr() == address);
        if (clientItem != null)
        {
            ViewModel.Server.Write(clientItem.Socket, buffer, 0, buffer.Length);
        }
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