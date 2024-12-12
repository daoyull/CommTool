using System.Net;
using System.Net.Sockets;
using Comm.Lib.Interface;
using Comm.Service.Messages;
using Comm.Service.Service;

namespace Comm.Service.IO;

public class UdpAdapter : AbstractCommunication<SocketMessage>, IUdp
{
    public UdpAdapter(IUdpConnectOption udpConnectOption, IUdpReceiveOption receiveOption, IUdpSendOption sendOption)
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

        if (string.IsNullOrEmpty(UdpSendOption.SendIp) || UdpSendOption.SendPort == 0)
        {
            return;
        }

        var ipEndPoint = new IPEndPoint(IPAddress.Parse(UdpSendOption.SendIp), UdpSendOption.SendPort);
        _client.Send(buffer.AsSpan().Slice(offset, count), ipEndPoint);
    }

    public override void Close()
    {
        if (Cts != null)
        {
            Cts.Cancel();
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

            _client = new UdpClient(new IPEndPoint(IPAddress.Parse(UdpConnectOption.Ip), UdpConnectOption.Port));
            OnConnected(new());

            Cts = new();
            var socketPipeHandle = new UdpReceiveTask(this, _client, Cts);
            Task.Run(socketPipeHandle.StartHandle, Cts.Token);
        }
        catch (Exception e)
        {
            Close();
            Ui.Logger.Warning($"连接失败: {e.Message}");
        }
    }
}