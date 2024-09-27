using Microsoft.ClearScript.V8;
using NetTool.Lib.Entity;
using NetTool.Lib.Interface;
using NetTool.Module.Share;
using Newtonsoft.Json;

namespace NetTool.Module.Components;

public class ScriptEngine : IJavaScriptExec
{
    private V8ScriptEngine _engine = new();
    private string? _script;

    public ScriptEngine()
    {
        _engine.AddHostType("Console", typeof(Console));
        _engine.AddHostType("ByteHelper", typeof(ByteHelper));
    }


    public void Reload(string script)
    {
        _script = script;
        _engine.Dispose();
        _engine = new();
        _engine.Execute(script);
    }

    public SendOption DoSend(byte[] buffer)
    {
        try
        {
            var result = _engine.Script.doSend(buffer);
            return JsonConvert.DeserializeObject<ReceiveOption>(JsonConvert.SerializeObject(result));
        }
        catch (Exception e)
        {
            return SendOption.Default;
        }
    }

    public ReceiveOption OnReceived(byte[] buffer)
    {
        try
        {
            var result = _engine.Script.onReceive(buffer);
            return JsonConvert.DeserializeObject<ReceiveOption>(JsonConvert.SerializeObject(result));
        }
        catch (Exception e)
        {
            return ReceiveOption.Default;
        }
    }

    public void Dispose()
    {
        _engine.Dispose();
    }
}