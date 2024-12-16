using Comm.Lib.Interface;
using Comm.Service.Messages;
using Comm.WPF.Abstracts;

namespace Comm.WPF.ViewModels
{
    internal class DesignViewModel : AbstractCommViewModel<SerialMessage>
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

        protected override void OnSendScript(byte[] buffer, string uiMessage)
        {
            throw new NotImplementedException();
        }

        protected override void OnReceiveScript(SerialMessage message, string uiMessage)
        {
            throw new NotImplementedException();
        }

        public override string ScriptType { get; }
    }
}
