using System.Windows;
using System.Windows.Controls;

namespace Comm.WPF.Components;

public partial class SendOptionView : UserControl
{
    public SendOptionView()
    {
        InitializeComponent();
    }

    private void DebugClick(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 10; i++)
        {
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
    }
}