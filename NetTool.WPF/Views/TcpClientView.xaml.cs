
using NetTool.WPF.ViewModels;

namespace NetTool.WPF.Views;

public partial class TcpClientView
{
    public TcpClientView()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {

        };
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        if (ViewModel != null)
        {
            ViewModel.UiLogger = NetLogger;
        }
       
    }
    
    
}