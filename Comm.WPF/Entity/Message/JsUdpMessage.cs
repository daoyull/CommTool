using System.Net;
using System.Net.Sockets;
using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Entity;

public readonly struct JsUdpMessage(ITypedArray<byte> data, DateTime time, string ip,int port)
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime time { get; } = time;

    /// <summary>
    /// 接收数据
    /// </summary>
    public ITypedArray<byte> data { get; } = data;

    public string ip { get; } = ip;

    public int port { get; } = port;
}