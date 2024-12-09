using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTool.Abstracts;
using NetTool.Lib.Abstracts;
using NetTool.Lib.Interface;
using NetTool.Models;
using NetTool.Module.Messages;

namespace NetTool.ViewModels
{
    internal class TestViewModel : AbstractNetViewModel<SerialPortMessage>
    {
        public override ICommunication<SerialPortMessage> Communication { get; }
        protected override Task Connect()
        {
            throw new NotImplementedException();
        }

        protected override void HandleReceiveMessage(SerialPortMessage message, string strMessage)
        {
            throw new NotImplementedException();
        }

        protected override void HandleSendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public override string ScriptType { get; }
    }
}
