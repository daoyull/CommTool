using System.Net;
using System.Net.Sockets;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Entity;

public readonly struct JsSocketMessage( ITypedArray<byte> data,Socket socket)
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime Time { get; } = DateTime.Now;

    /// <summary>
    /// 接收数据
    /// </summary>
    public ITypedArray<byte> Data { get; } = data;

    /// <summary>
    /// Socket
    /// </summary>
    public Socket Socket { get; } = socket;

    public string Ip => ((IPEndPoint)Socket.LocalEndPoint!).Address.ToString();
    public int Port => ((IPEndPoint)Socket.LocalEndPoint!).Port;
}