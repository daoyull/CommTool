namespace Comm.WPF.Abstracts;

public abstract partial class AbstractCommViewModel<T>
{
    private CancellationTokenSource? _autoSendCts;

    protected virtual async Task? AutoSendMethod()
    {
        while (IsConnect && SendOption.AutoSend && _autoSendCts is { IsCancellationRequested: false })
        {
            if (Ui != null)
            {
                var message = Ui.SendMessage;
                await SendMessage(message);
            }

            await Task.Delay(SendOption.AutoSendTime);
        }
    }
}