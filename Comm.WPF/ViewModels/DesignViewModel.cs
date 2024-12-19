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

        protected override void LogReceiveMessage(SerialMessage message, string strMessage)
        {
            throw new NotImplementedException();
        }

        protected override void LogSendMessage(byte[] bytes,string message)
        {
            throw new NotImplementedException();
        }

        protected override void InvokeSendScript(byte[] buffer, string uiMessage)
        {
            throw new NotImplementedException();
        }

        protected override object InvokeReceiveScript(SerialMessage message)
        {
            throw new NotImplementedException();
        }

      

        protected override string ScriptType { get; }
    }
}
