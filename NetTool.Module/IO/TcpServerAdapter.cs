// using System.Net.Sockets;
// using NetTool.Lib.Abstracts;
// using NetTool.Lib.Interface;
// using NetTool.Module.Messages;
//
// namespace NetTool.Module.IO;
//
// public class TcpServerAdapter : AbstractCommunication<TcpServerMessage>, ITcpServer
// {
//     public TcpServerAdapter(INotify notify, IGlobalOption globalOption, ITcpServerOption tcpServerOption,
//         ITcpServerReceiveOption serverReceiveOption, ITcpServerSendOption serverSendOption) : base(notify, globalOption)
//     {
//         TcpServerOption = tcpServerOption;
//         TcpServerReceiveOption = serverReceiveOption;
//         TcpServerSendOption = serverSendOption;
//     }
//
//     private Socket? _server;
//
//     public override IReceiveOption ReceiveOption => TcpServerReceiveOption;
//     public override ISendOption SendOption => TcpServerSendOption;
//
//     protected override int Read(ref byte[] buffer)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override bool CanReadData()
//     {
//         return true;
//     }
//
//     public override void Write(byte[] buffer, int offset, int count)
//     {
//         throw new NotImplementedException();
//     }
//
//     protected override void WriteMessage(byte[] bytes)
//     {
//         throw new NotImplementedException();
//     }
//
//     public void Listen()
//     {
//         _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//         _server.Listen();
//     }
//     public ITcpServerOption TcpServerOption { get; }
//     public ITcpServerReceiveOption TcpServerReceiveOption { get; }
//     public ITcpServerSendOption TcpServerSendOption { get; }
// }