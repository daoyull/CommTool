using System.Configuration;
using System.Data;
using System.Windows;
using Comm.WPF.Common;

namespace TcpServer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        AppCommon.CreateAppCommon();
    }
}