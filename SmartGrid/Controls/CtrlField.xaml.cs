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
        private void SpltH1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.H1 + WorkSpace.Instance.ActiveField.GridWidth.H2;
            WorkSpace.Instance.ActiveField.GridWidth.H1 = WorkSpace.Instance.ActiveField.GridWidth.H2 = all / 2;
        }

        private void SpltH2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.H2 + WorkSpace.Instance.ActiveField.GridWidth.H3;
            WorkSpace.Instance.ActiveField.GridWidth.H2 = WorkSpace.Instance.ActiveField.GridWidth.H3 = all / 2;
        }

        private void SpltW1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.W1 + WorkSpace.Instance.ActiveField.GridWidth.W2;
            WorkSpace.Instance.ActiveField.GridWidth.W1 = WorkSpace.Instance.ActiveField.GridWidth.W2 = all / 2;
        }

        private void SpltW2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.W2 + WorkSpace.Instance.ActiveField.GridWidth.W3;
            WorkSpace.Instance.ActiveField.GridWidth.W2 = WorkSpace.Instance.ActiveField.GridWidth.W3 = all / 2;
        }

    }
}
