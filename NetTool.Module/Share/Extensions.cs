using Common.Lib.Ioc;
using NetTool.Lib.Interface;
using NetTool.Module.Components;

namespace NetTool.Module.Share;

public static partial class Extensions
{
    public static string BytesToString(this byte[] bytes)
    {
        var encoding = Ioc.Resolve<IGlobalOption>().Encoding;
        return encoding.GetString(bytes);
    }

    public static byte[] StringToBytes(this string str)
    {
        var encoding = Ioc.Resolve<IGlobalOption>().Encoding;
        return encoding.GetBytes(str);
    }

    public static string BytesToHexString(this byte[] bytes, string delimiter = " ")
    {
        var builderPool = Ioc.Resolve<StringBuilderPool>();
        var builder = builderPool.Rent();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append($"{bytes[i]:X2}{delimiter}");
        }

        var trim = builder.ToString().Trim();
        builderPool.Return(builder);
        return trim;
    }

    public static byte[] HexStringToBytes(this string hexStr)
    {
        var filteredHexStr = new string(hexStr.Where(c => HexChars.Contains(c)).ToArray());
        if (filteredHexStr.Length % 2 != 0)
        {
            // 在倒数第二位插入'0'
            filteredHexStr = filteredHexStr.Insert(filteredHexStr.Length - 1, "0");
        }

        return Convert.FromHexString(filteredHexStr);
    }

    public static string HexStringToString(this string hexStr)
    {
        var bytes = hexStr.HexStringToBytes();
        return bytes.BytesToString();
    }

    public static string StringToHexString(this string encodingStr)
    {
        var bytes = encodingStr.StringToBytes();
        return bytes.BytesToHexString();
    }
}

public static partial class Extensions
{
    private static readonly HashSet<char> HexChars =
        ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f'];
}