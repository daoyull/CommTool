using System.Windows.Controls;

namespace Comm.WPF.Views;

public partial class UdpView
{
    public UdpView()
    {
        InitializeComponent();
        ViewModel!.Ui = ViewModel.Communication.Ui = CommUi;
    }
}