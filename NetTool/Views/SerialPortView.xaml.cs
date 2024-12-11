using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetTool.Views;

public partial class SerialPortView
{
    public SerialPortView()
    {
        InitializeComponent();
        ViewModel!.Ui = ViewModel.Communication.Ui = NetUiComponent;
    }
}