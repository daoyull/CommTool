﻿using NetTool.Lib.Interface;

namespace NetTool.Module.Messages;

public class SerialPortMessage : IMessage
{
    public SerialPortMessage(byte[] data)
    {
        Data = data;
    }

    public DateTime Time { get; } = DateTime.Now;
    public byte[] Data { get; }
}