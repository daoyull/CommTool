using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NetTool.Views;

public partial class TcpClientView
{
    public TcpClientView()
    {
        InitializeComponent();
        ViewModel!.Ui = ViewModel.Communication.Ui = NetUiComponent;
    }
}