using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Interaction logic for ExpandButton.xaml
    /// </summary>
    public partial class ExpandButton : UserControl
    {
        public ExpandButton()
        {
            InitializeComponent();
            btnExpand.DataContext = this;
        }
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool),
                typeof(ExpandButton), new FrameworkPropertyMetadata()
                    { DefaultValue = false, BindsTwoWayByDefault = true});
    }
}
