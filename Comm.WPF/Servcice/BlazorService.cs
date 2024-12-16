using BlazorMonaco.Editor;
using Microsoft.JSInterop;

namespace Comm.WPF.Servcice;

public class BlazorService
{
    public IJSRuntime? JsRuntime { get; set; }
    public StandaloneCodeEditor? Editor { get; set; }

    public string? Content { get; set; }

    public void Start()
    {
        _cts = new();
        Task.Run(StartSetValue, _cts.Token);
    }

    public void Close()
    {
        _cts?.Dispose();
        _cts = null;
        JsRuntime = null;
        Editor = null;
    }

    private CancellationTokenSource? _cts;

    public async Task StartSetValue()
    {
        // monaco加载需要一定时间
        try
        {
            while (!_cts!.IsCancellationRequested)
            {
                if (Content == null || Editor == null || JsRuntime == null)
                {
                    await Task.Delay(200);
                    continue;
                }

                await Editor.SetValue(Content);
                Content = null;
            }
        }catch(OperationCanceledException)
        {
            // ignore
        }
    }
}