using NetTool.Lib.Entity;

namespace NetTool.Lib.Interface;

public interface IJavaScriptExec
{
    public void Reload(string script);

    public SendOption DoSend(byte[] buffer);

    public ReceiveOption OnReceived(byte[] buffer);
    
}