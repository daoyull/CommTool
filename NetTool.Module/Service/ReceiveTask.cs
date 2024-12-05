using System.Diagnostics;
using NetTool.Lib.Interface;

namespace NetTool.Module.Service;

public class ReceiveTask
{
    private readonly Stopwatch _stopwatch = new();
    private readonly List<byte> _list = new();
    
    public async Task StartTask(IReceiveTask receiveTask,Action<byte[]> handle, Action closeHandle, CancellationToken cts)
    {
        while (cts is { IsCancellationRequested: false })
        {
            if (receiveTask.TimeBreak)
            {
                
            }
            else
            {
                // 固定字节分
                var maxSize = receiveTask.MaxByteSize;
                var canReadByte = receiveTask.CanReadByte;
                if (canReadByte > maxSize)
                {
                    
                }
            }
        }
    }
}