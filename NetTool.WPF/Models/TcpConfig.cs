using CommunityToolkit.Mvvm.ComponentModel;

namespace NetTool.WPF.Models;

public partial class TcpConfig : ObservableObject
{
    [ObservableProperty] private string? _ip = "127.0.0.1";
    [ObservableProperty] private uint _port = 7789;

    public bool Verify(out string errMsg)
    {
        errMsg = string.Empty;
        if (string.IsNullOrEmpty(Ip))
        {
            errMsg = "IP地址不能为空";
            return false;
        }

        if (Port == 0)
        {
            errMsg = "端口不能为空";
            return false;
        }

        return true;
    }
}