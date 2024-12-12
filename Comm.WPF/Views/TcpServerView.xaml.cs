using System.Windows.Controls;

namespace Comm.WPF.Views;

public partial class TcpServerView
{
    public TcpServerView()
    {
        InitializeComponent();
        ViewModel!.Ui = ViewModel.Communication.Ui = CommUi;
    }
}