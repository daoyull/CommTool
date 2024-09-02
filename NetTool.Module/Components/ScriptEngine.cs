using Microsoft.ClearScript.V8;
using NetTool.Lib.Entity;
using NetTool.Lib.Interface;
using NetTool.Module.Common;
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

    public string? Script
    {
        get => _script;
        set
        {
            _script = value;
            if (!string.IsNullOrEmpty(value))
            {
                _engine.Execute(value);
            }
        }
    }


    public void Reload(string script)
    {
        Script = script;
    }

    public SendOption DoSend(byte[] buffer)
    {
        if (string.IsNullOrEmpty(Script))
        {
            return SendOption.Default;
        }

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
        if (string.IsNullOrEmpty(Script))
        {
            return ReceiveOption.Default;
        }

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