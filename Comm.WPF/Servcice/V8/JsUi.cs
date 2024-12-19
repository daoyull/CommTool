using Comm.Lib.Interface;
using Comm.WPF.Abstracts;

namespace Comm.WPF.Servcice.V8;

public class JsUi<T> where T : IMessage
{
    public AbstractCommViewModel<T> ViewModel { get; }


    public JsUi(AbstractCommViewModel<T> viewModel)
    {
        ViewModel = viewModel;
    }

    public void logInfo(string message)
    {
        ViewModel.Ui.Logger.Info(message);
    }

    public void logPrimary(string message)
    {
        ViewModel.Ui.Logger.Primary(message);
    }

    public void logSuccess(string message)
    {
        ViewModel.Ui.Logger.Success(message);
    }

    public void logWaring(string message)
    {
        ViewModel.Ui.Logger.Warning(message);
    }

    public void logError(string message)
    {
        ViewModel.Ui.Logger.Error(message);
    }

    public void log(string message, string color)
    {
        ViewModel.Ui.Logger.Write(message, color);
    }

    public void logEmptyLine()
    {
        ViewModel.Ui.Logger.WriteEmptyLine();
    }

    public void clearLog()
    {
        ViewModel.Ui.Logger.ClearArea();
    }

    public void addSendFrame(int num)
    {
        ViewModel.Ui.AddSendFrame((uint)num);
    }

    public void addReceiveFrame(int num)
    {
        ViewModel.Ui.AddReceiveFrame((uint)num);
    }

    public void addSendBytes(int num)
    {
        ViewModel.Ui.AddSendBytes((uint)num);
    }

    public void addReceiveBytes(int num)
    {
        ViewModel.Ui.AddReceiveBytes((uint)num);
    }

    public void resetNumber()
    {
        ViewModel.Ui.ResetNumber();
    }

    public void scrollToEnd()
    {
        ViewModel.Ui.ScrollToEnd();
    }
}