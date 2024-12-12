using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using Comm.Lib.Interface;
using Comm.Service.IO;
using Comm.Service.Messages;
using Comm.Service.Share;

namespace Comm.Service.Service;

public class UdpReceiveTask(ICommunication<SocketMessage> communication, UdpClient client, CancellationTokenSource cts)
    : IReceiveTask<SocketMessage>
{
    public UdpClient Client { get; } = client;

    public Task StartHandle()
    {
        // 启动接收
        Task.Run(ReceiveTask, cts.Token);
        return Task.CompletedTask;
    }

    public ICommunication<SocketMessage> Communication { get; } = communication;

    private async Task ReceiveTask()
    {
        try
        {
            while (!cts.IsCancellationRequested)
            {
                IPEndPoint? ipEndPoint = null;
                var buffer = Client.Receive(ref ipEndPoint);
                var socketMessage = new SocketMessage(buffer, ipEndPoint.ToString());

                if (Communication is AbstractCommunication<SocketMessage> socketCommunication)
                {
                    await socketCommunication.WriteMessageAsync(socketMessage);
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }
}