using NetTool.Lib.Args;

namespace NetTool.Lib.Interface;

public interface INetService : IDisposable
{
    #region 事件

    event EventHandler<ReceiveArgs> Received;

    event EventHandler<ClosedArgs> Closed;

    event EventHandler<ConnectedArgs> Connected;

    #endregion

    public bool IsConnect { get;  }
}