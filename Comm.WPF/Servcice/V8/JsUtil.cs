using Comm.Service.Share;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace Comm.WPF.Servcice.V8;

public class JsUtil
{
    private readonly V8ScriptEngine _engine;

    public JsUtil(V8ScriptEngine engine)
    {
        _engine = engine;
    }

    public string arrayToString(ITypedArray<byte> buffer, bool isHex = false)
    {
        return isHex ? buffer.ToArray().BytesToHexString() : buffer.ToArray().BytesToString();
    }

    public ITypedArray<byte> stringToArray(string message, bool isHex = false)
    {
        byte[] buffer = isHex ? message.HexStringToBytes() : message.StringToBytes();
        var array = (ITypedArray<byte>)_engine.Invoke("arrayToUint8Array", buffer);
        return array;
    }
}