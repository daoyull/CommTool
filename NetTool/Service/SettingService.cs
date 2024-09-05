using System.Text;
using NetTool.Module.Common;

namespace NetTool.Service;

public class SettingService
{
    Encoding _currentEncoding = Encoding.UTF8;


    public string BytesToString(byte[] bytes)
    {
        return _currentEncoding.GetString(bytes);
    }

    public byte[] StringToBytes(string str)
    {
        return _currentEncoding.GetBytes(str);
    }

    public string ToHexStr(string str)
    {
        return StringToBytes(str).ToHexString();
    }

    public string HexToStr(string str)
    {
        return BytesToString(str.HexStringToArray());
    }
}