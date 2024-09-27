namespace NetTool.Lib.Interface;

public interface INetUi
{
    public IGlobalOption GlobalOption { get; }

    public IUiLogger Logger { get; }
    public INotify Notify { get; }

    public ISendOption SendOption { get; set; }
    public IReceiveOption ReceiveOption { get; set; }

    public void AddSendFrame(uint add);
    public void AddReceiveFrame(uint add);
    public void AddSendBytes(uint add);
    public void AddReceiveBytes(uint add);
    public void ResetNumber();

    public void WriteReceive(IMessage message);
    public void WriteSend(IMessage message);
}