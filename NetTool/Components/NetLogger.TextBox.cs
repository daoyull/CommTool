// using System.Windows;
// using System.Windows.Controls;
// using NetTool.Lib.Interface;
//
// namespace NetTool.Components;
//
// public class NetLogger : TextBox,IUiLogger
// {
//     public NetLogger()
//     {
//         AcceptsReturn = true;
//         TextWrapping = TextWrapping.Wrap;
//                  VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
//          HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
//     }
//     public void Message(string message, string color)
//     {
//         Dispatcher.Invoke(() =>
//         {
//             Text += message;
//             ScrollToEnd();
//         });
//     }
//
//     public void Info(string message)
//     {
//         Message(message, string.Empty);
//     }
//
//     public void Success(string message)
//     {
//         Message(message, string.Empty);
//     }
//
//     public void Warning(string message)
//     {
//         Message(message, string.Empty);
//     }
//
//     public void Error(string message)
//     {
//         Message(message, string.Empty);
//     }
// }