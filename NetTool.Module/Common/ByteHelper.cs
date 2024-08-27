using System.Text;

namespace NetTool.Module.Common;

public static class ByteHelper
{
    public static string ToUtf8Str(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}