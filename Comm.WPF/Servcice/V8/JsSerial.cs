using Comm.WPF.ViewModels;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Servcice.V8;

public class JsSerial
{
    private readonly V8ScriptEngine _engine;
    public SerialPortViewModel ViewModel { get; }

    public JsSerial(SerialPortViewModel viewModel, V8ScriptEngine engine)
    {
        _engine = engine;
        ViewModel = viewModel;
    }

    public void send(string message)
    {
        var buffer = ViewModel.StringToBuffer(message);
        sendBuffer(buffer);
    }

    public void sendBuffer(ITypedArray<byte> buffer)
    {
        sendBuffer(buffer.ToArray());
    }

    public void sendBuffer(byte[] buffer)
    {
        ViewModel.Communication.Write(buffer, 0, buffer.Length);
    }
    
}