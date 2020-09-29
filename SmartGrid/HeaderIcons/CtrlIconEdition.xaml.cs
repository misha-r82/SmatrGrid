using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace SmartGrid.HeaderIcons
{
    /// <summary>
    /// Логика взаимодействия для IconTemplate.xaml
    /// </summary>
    public partial class CtrlIconEdition : UserControl
    {
        public CtrlIconEdition()
        {
            InitializeComponent();
        }

        public HeaderIcon SelectedIcon
        {
            get
            {
                return iconTree.SelectedItem as HeaderIcon;
                
            }
        }

        public HeaderIcon Icon { get => DataContext as HeaderIcon; }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CtrlIconEdition_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = e.NewValue;
        }

    }
}
