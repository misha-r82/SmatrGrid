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

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SmartFiled _field;
        public MainWindow()
        {
            InitializeComponent();
            _field = new SmartFiled();
            tag00.DataContext = _field.WorkTag;
            tag11.DataContext = _field.Cells[0, 0];
            tag12.DataContext = _field.Cells[0, 1];
            tag13.DataContext = _field.Cells[0, 2];
            tag21.DataContext = _field.Cells[1, 0];
            tag22.DataContext = _field.Cells[1, 1];
            tag23.DataContext = _field.Cells[1, 2];
            tag31.DataContext = _field.Cells[2, 0];
            tag32.DataContext = _field.Cells[2, 1];
            tag33.DataContext = _field.Cells[2, 2];
        }

    }
}
