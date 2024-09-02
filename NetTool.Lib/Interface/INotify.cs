namespace NetTool.Lib.Interface;

public interface INotify
{
    void Info(string message);

    void Success(string message);

    void Warning(string message);

    void Error(string message);
}