﻿using System.Net;
using System.Net.Sockets;
using Comm.Lib.Interface;

namespace Comm.Service.Messages;

public readonly struct SocketMessage(byte[] data, Socket socket) : IMessage
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime Time { get; } = DateTime.Now;
    
    /// <summary>
    /// 接收数据
    /// </summary>
    public byte[] Data { get; } = data;
    
    /// <summary>
    /// 接收数据地址
    /// </summary>
    public string RemoteIp  => $"{Ip}:{Port}";

    /// <summary>
    /// Socket
    /// </summary>
    public Socket Socket { get; } = socket;

    public string Ip => ((IPEndPoint)Socket.LocalEndPoint!).Address.ToString();
    public int Port => ((IPEndPoint)Socket.LocalEndPoint!).Port;
}