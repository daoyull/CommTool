using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace NetTool.Views;

public partial class ScriptManagerView : Window
{
    public ScriptManagerView()
    {
        InitializeComponent();
        Loaded += HandleLoaded;
    }

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        

    }
}