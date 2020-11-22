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
        private void CtrlIconEdition_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = e.NewValue;
        }

        private void Icon_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2) return;
            var icon = ((sender as FrameworkElement)?.DataContext) as HeaderIcon;
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;

            try
            {
                if (fileDialog.ShowDialog() != true) return;
                var stream = new FileStream(fileDialog.FileName, FileMode.Open);
                icon.Icon.FromStream(stream);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка чтения файла {fileDialog.FileName}", "Ошибка чтения", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            

        }
    }
}
