using System.Net;
using System.Net.Sockets;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Entity;

public readonly struct JsSocketMessage(ITypedArray<byte> data, DateTime time, Socket socket,bool isLocal)
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime time { get; } = time;

    /// <summary>
    /// 接收数据
    /// </summary>
    public ITypedArray<byte> data { get; } = data;

    /// <summary>
    /// Socket
    /// </summary>
    public Socket Socket { get; } = socket;

    public string ip
    {
        get
        {
            if (isLocal)
            {
                return ((IPEndPoint)Socket.LocalEndPoint!).Address.ToString();
            }
            return ((IPEndPoint)Socket.RemoteEndPoint!).Address.ToString();
        }
    }

    public int port
    {
        get
        {
            if (isLocal)
            {
                return ((IPEndPoint)Socket.LocalEndPoint!).Port;
            }
            return ((IPEndPoint)Socket.RemoteEndPoint!).Port;
        }
    }
}