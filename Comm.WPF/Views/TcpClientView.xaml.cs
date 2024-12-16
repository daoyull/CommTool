using Comm.WPF.ViewModels;
using Common.Lib.Ioc;

namespace Comm.WPF.Views;

public partial class TcpClientView
{
    public TcpClientView()
    {
        InitializeComponent();
        var viewModel = Ioc.ResolveOptional<TcpClientViewModel>();
        DataContext = viewModel;
        viewModel!.Ui = viewModel.Communication.Ui = CommUi;
    }
}