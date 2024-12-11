using System.Net.Sockets;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;
using NetTool.Module.Service;

namespace NetTool.Module.IO;

public class UdpAdapter : AbstractCommunication<SocketMessage>, IUdp
{
    public UdpAdapter(INotify notify, IGlobalOption globalOption,
        IUdpConnectOption udpConnectOption, IUdpReceiveOption receiveOption, IUdpSendOption sendOption) : base(notify,
        globalOption)
    {
        UdpConnectOption = udpConnectOption;
        UdpReceiveOption = receiveOption;
        UdpSendOption = sendOption;
    }

    #region Option

    public override IConnectOption ConnectOption => UdpConnectOption;
    public override IReceiveOption ReceiveOption => UdpReceiveOption;
    public override ISendOption SendOption => UdpSendOption;

    public IUdpConnectOption UdpConnectOption { get; set; }
    public IUdpReceiveOption UdpReceiveOption { get; set; }
    public IUdpSendOption UdpSendOption { get; set; }

    #endregion

    #region 字段

    private UdpClient? _client;

    #endregion

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!IsConnect || _client == null)
        {
            Notify.Warning("Udp Not Open");
            return;
        }

        var sendBuffer = buffer[..offset][count..];
        _client.Send(sendBuffer);
    }

    public override void Close()
    {
        if (Cts != null)
        {
            Cts.Dispose();
            Cts = null;
        }

        if (_client != null)
        {
            _client.Dispose();
            _client = null;
        }
    }


    public override void Connect()
    {
        try
        {
            if (IsConnect)
            {
                Close();
            }

            if (string.IsNullOrEmpty(UdpConnectOption.Ip) || UdpConnectOption.Port <= 0)
            {
                throw new Exception("Ip or Port is null");
            }

            _client = new UdpClient();
            _client.Connect(UdpConnectOption.Ip, UdpConnectOption.Port);
            OnConnected(new());
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Close();
        }
    }

    private void HandleFrameReceive(object? sender, byte[] e)
    {
        Console.WriteLine($"Udp接收到{e.Length}字节数据");
        WriteMessage(new(e, ""));
    }
}