using TouchSocket.Core;

namespace NetTool.Lib.Interface;

public interface IJavaScriptExec
{
    public string? Script { get; set; }
    
    public object DoSend(byte[] buffer);

    public object Received(ByteBlock byteBlock);
}