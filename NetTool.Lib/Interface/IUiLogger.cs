namespace NetTool.Lib.Interface;

/// <summary>
/// 界面日志
/// </summary>
public interface IUiLogger
{
    public void Write(string message, string color);

    public void Info(string message);

    public void Success(string message);

    public void Warning(string message);

    public void Error(string message);

    public void ClearArea();
}