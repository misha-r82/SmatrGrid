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

namespace SmartGrid.Grid
{
    /// <summary>
    /// Логика взаимодействия для CtrlGridWidthCommands.xaml
    /// </summary>
    public partial class CtrlGridWidthCommands : UserControl
    {
        public CtrlGridWidthCommands()
        {
            InitializeComponent();
        }
        private WidthManager Wm => WorkSpace.Instance.ActiveField.GridWidth.Manager;
        private void BtnShowAll_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowAll();
        }

        private void BtnHideV1_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideW1();
        }
        private void BtnHideV2_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideW2();
        }
        private void BtnHideV3_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideW3();
        }
        private void BtnHideH1_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideH1();
        }
        private void BtnHideH2_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideH2();
        }
        private void BtnHideH3_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.ShowHideH3();
        }

        private void BtnHorAlign_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.HorAlign();
        }

        private void BtnVertAlign_OnClick(object sender, RoutedEventArgs e)
        {
            Wm.VertAlign();
        }
    }
}
