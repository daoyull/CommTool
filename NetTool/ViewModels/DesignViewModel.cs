using NetTool.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Module.Messages;

namespace NetTool.ViewModels
{
    internal class DesignViewModel : AbstractNetViewModel<SerialMessage>
    {
        public override ICommunication<SerialMessage> Communication { get; }
        protected override Task Connect()
        {
            throw new NotImplementedException();
        }

        protected override void HandleReceiveMessage(SerialMessage message, string strMessage)
        {
            throw new NotImplementedException();
        }

        protected override void HandleSendMessage(byte[] bytes,string message)
        {
            throw new NotImplementedException();
        }

        public override string ScriptType { get; }
    }
}
