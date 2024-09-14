using System.Threading.Channels;
using NetTool.Lib.Args;
using NetTool.Lib.Messages;

namespace NetTool.Lib.Interface;

public interface INetService : IDisposable
{
    #region 事件
    

    event EventHandler<ClosedArgs> Closed;

    event EventHandler<ConnectedArgs> Connected;

    #endregion

    public Action<ReceiveMessage> ReceiveMessageAction { get; set; }

    public bool IsConnect { get; }
}