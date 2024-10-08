using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NetTool.Views;

public partial class TcpClientView
{
    public TcpClientView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        if (ViewModel != null)
        {
            ViewModel.Ui = ComCompenent;
        }
    }
}