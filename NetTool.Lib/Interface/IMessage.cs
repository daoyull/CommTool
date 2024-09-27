namespace NetTool.Lib.Interface;

public interface IMessage
{
    public DateTime Time { get; }

    public byte[] Data { get; }

    public void ReceiveDisplay(INetUi ui);

    public void SendDisplay(INetUi ui);
}