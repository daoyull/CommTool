using NetTool.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.Module.IO;

public class SerialPortAdapter : AbstractCommunication<SerialPortMessage>
{
    public override void Write(byte[] buffer, int offset, int count)
    {
        
    }
}