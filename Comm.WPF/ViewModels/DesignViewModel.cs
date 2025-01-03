﻿using Comm.Lib.Interface;
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

        protected override void LogUiReceiveMessage(SerialMessage message)
        {
            throw new NotImplementedException();
        }

        protected override void LogFileReceiveMessage(SerialMessage message)
        {
            throw new NotImplementedException();
        }

        protected override void LogSendMessage(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        protected override void LogFileSendMessage(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override object InvokeSendScript(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        protected override object InvokeReceiveScript(SerialMessage message)
        {
            throw new NotImplementedException();
        }

      

        protected override string Type { get; }
    }
}
