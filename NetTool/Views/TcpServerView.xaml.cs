using System.Windows.Controls;

namespace NetTool.Views;

public partial class TcpServerView 
{
    public TcpServerView()
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