namespace NetTool.Lib.Interface;

public interface IPipeHandle<T> where T : IMessage
{
    /// <summary>
    /// 开始处理
    /// </summary>
    /// <returns></returns>
    public Task StartHandle();

    public ICommunication<T> Communication { get;}
    
}