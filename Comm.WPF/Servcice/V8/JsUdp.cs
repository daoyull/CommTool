using Comm.Service.Share;
using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Servcice.V8;

public class JsUdp
{
    public UdpViewModel ViewModel { get; }

    public JsUdp(UdpViewModel viewModel, V8ScriptEngine engine)
    {
        ViewModel = viewModel;
    }


    public void sendBuffer(string address, byte[] buffer)
    {
        if ($"{ViewModel.UdpAdapter.UdpConnectOption.Ip}:{ViewModel.UdpAdapter.UdpConnectOption.Port}" == address)
        {
            ViewModel.Ui.Logger.Warning("$脚本中不允许像本机发送信息,否则可能出现连续调用无限触发脚本的问题!");
            return;
        }

        ViewModel.UdpAdapter.Write(address, buffer).Wait();
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