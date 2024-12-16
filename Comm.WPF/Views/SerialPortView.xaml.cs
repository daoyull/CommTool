using Comm.WPF.ViewModels;
using Common.Lib.Ioc;

namespace Comm.WPF.Views;

public partial class SerialPortView
{
    public SerialPortView()
    {
        InitializeComponent();
        var viewModel = Ioc.ResolveOptional<SerialPortViewModel>();
        DataContext = viewModel;
        viewModel!.Ui = viewModel.Communication.Ui = CommUi;
    }
}