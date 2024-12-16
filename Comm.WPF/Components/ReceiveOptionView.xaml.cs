using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Comm.WPF.Components
{
    /// <summary>
    /// ReceiveOptionView.xaml 的交互逻辑
    /// </summary>
    public partial class ReceiveOptionView : UserControl
    {
        public ReceiveOptionView()
        {
            InitializeComponent();
        }



        public bool IsShowSplitPackage
        {
            get { return (bool)GetValue(IsShowSplitPackageProperty); }
            set { SetValue(IsShowSplitPackageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowSplitPackage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowSplitPackageProperty =
            DependencyProperty.Register("IsShowSplitPackage", typeof(bool), typeof(ReceiveOptionView), new PropertyMetadata(true));


    }
}
