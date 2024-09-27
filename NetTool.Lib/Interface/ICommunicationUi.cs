namespace NetTool.Lib.Interface;

public interface ICommunicationUi
{
    public IUiLogger Logger { get; }
    public INotify Notify { get; }

    public void AddSendFrame(uint add);
    public void AddReceiveFrame(uint add);
    public void AddSendBytes(uint add);
    public void AddReceiveBytes(uint add);
    public void ResetNumber();
}