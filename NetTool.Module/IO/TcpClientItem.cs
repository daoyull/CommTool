using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace NetTool.Module.IO;

public class TcpClientItem
{
    public CancellationTokenSource Cts { get; } = new();
    public ReceiveSocket ReceiveSocket { get; set; } = null!;
}