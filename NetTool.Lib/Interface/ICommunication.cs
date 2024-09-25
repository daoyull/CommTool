﻿using NetTool.Lib.Args;

namespace NetTool.Lib.Interface;

public interface ICommunication<T> : IDisposable where T : IMessage
{
    #region 事件

    event EventHandler<ClosedArgs> Closed;

    event EventHandler<ConnectedArgs> Connected;

    #endregion

    IAsyncEnumerable<T> MessageReadAsync();

    public void Write(byte[] buffer, int offset, int count);

    public Task WriteAsync(byte[] buffer, int offset, int count);
}