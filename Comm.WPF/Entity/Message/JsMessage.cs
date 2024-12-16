using Microsoft.ClearScript.JavaScript;

namespace Comm.WPF.Entity;

public readonly struct JsMessage(ITypedArray<byte> data)
{
    /// <summary>
    /// 接收时间
    /// </summary>
    public DateTime Time { get; } = DateTime.Now;

    /// <summary>
    /// 接收数据
    /// </summary>
    public ITypedArray<byte> Data { get; } = data;
}