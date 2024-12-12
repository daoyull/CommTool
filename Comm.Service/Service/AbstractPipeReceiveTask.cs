using System.Diagnostics;
using System.IO.Pipelines;
using Comm.Lib.Interface;

namespace Comm.Service.Service;

public abstract class AbstractPipeReceiveTask<T>(ICommunication<T> communication, CancellationTokenSource cts)
    : IReceiveTask<T> where T : IMessage
{
    protected readonly Stopwatch Stopwatch = new();
    public CancellationTokenSource Cts { get; } = cts;
    private Pipe Pipe { get; } = new();

    protected PipeReader Reader => Pipe.Reader;

    protected PipeWriter Writer => Pipe.Writer;

    public abstract Task StartHandle();
    public ICommunication<T> Communication { get; } = communication;

    public IReceiveOption ReceiveOption => Communication.ReceiveOption;
}