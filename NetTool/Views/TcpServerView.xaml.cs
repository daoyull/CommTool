using System.Windows.Controls;

namespace NetTool.Views;

public partial class TcpServerView
{
    public TcpServerView()
    {
        InitializeComponent();
        ViewModel!.Ui = ViewModel.Communication.Ui = NetUiComponent;
    }
}