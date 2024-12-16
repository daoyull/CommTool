using Comm.WPF.ViewModels;
using Common.Lib.Ioc;

namespace Comm.WPF.Views;

public partial class TcpServerView
{
    public TcpServerView()
    {
        InitializeComponent();
        var viewModel = Ioc.ResolveOptional<TcpServerViewModel>();
        DataContext = viewModel;
        viewModel!.Ui = viewModel.Communication.Ui = CommUi;
    }
}