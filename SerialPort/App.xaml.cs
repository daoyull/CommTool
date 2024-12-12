using System.Windows;
using Comm.WPF.Common;

namespace SerialPort;

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