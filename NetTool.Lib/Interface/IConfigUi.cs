using NetTool.Lib.Entity;

namespace NetTool.Lib.Interface;

public interface IConfigUi
{
    public object Content { get; }

    public ConfigType ConfigType { get; set; }

    public IConnectOption ConnectOption { get; }

    public IReceiveOption ReceiveOption { get; }
    
    public ISendOption SendOption { get; }
}