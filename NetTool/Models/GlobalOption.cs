using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using NetTool.Lib.Interface;
using Path = System.IO.Path;

namespace NetTool.Models;

public partial class GlobalOption : ObservableObject, IGlobalOption
{
    /// <summary>
    /// 缓冲区大小
    /// </summary>
    [ObservableProperty] private int _bufferSize = 1024;
    /// <summary>
    /// 脚本默认地址
    /// </summary>
    [ObservableProperty] private string _scriptPath = Path.Combine(AppContext.BaseDirectory, "script");
    
    /// <summary>
    /// 默认编码
    /// </summary>
    [ObservableProperty] private Encoding _encoding = Encoding.ASCII;
}