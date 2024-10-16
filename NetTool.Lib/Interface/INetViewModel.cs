namespace NetTool.Lib.Interface;

public interface INetViewModel
{
    void HandleMessage<T>(T message) where T : IMessage;

    public INet Net { get; set; }
}