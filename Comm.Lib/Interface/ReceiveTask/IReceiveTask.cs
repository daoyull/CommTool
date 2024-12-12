namespace Comm.Lib.Interface;

public interface IReceiveTask<T> where T : IMessage
{
    /// <summary>
    /// 开始处理
    /// </summary>
    /// <returns></returns>
    public Task StartHandle();

    /// <summary>
    /// 通讯服务
    /// </summary>
    public ICommunication<T> Communication { get;}
    
}