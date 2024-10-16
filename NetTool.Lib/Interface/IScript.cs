using NetTool.Lib.Entity;

namespace NetTool.Lib.Interface;

public interface IScript : IDisposable
{
    public void Reload(string script);
}