using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SmartGrid.Drag;
using SmartGrid.Grid;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для CtrlField.xaml
    /// </summary>
    public partial class CtrlField : UserControl
    {
        public CtrlField()
        {
            InitializeComponent();
        }

        private WidthManager Wm => WorkSpace.Instance.ActiveField.GridWidth.Manager;
        private void SpltH1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wm.MiddleH12();
        }

        private void SpltH2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wm.MiddleH23();
        }

        private void SpltW1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wm.MiddleW12();
        }

        private void SpltW2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wm.MiddleW23();
        }

        private void CtrlField_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
