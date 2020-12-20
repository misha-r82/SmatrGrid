using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using SmartGrid.Controls;

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

        public IconElement SelectedIcon
        {
            get
            {
                return iconTree.SelectedItem as IconElement;
            }
        }
        private void CtrlIconEdition_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = e.NewValue;
        }

        private void Icon_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2 || e.Source is EditableLable) return;
            var datacontext = (sender as FrameworkElement)?.DataContext;
            var icon = datacontext as IconElement;
            if (icon == null) icon = ((IconCollection) datacontext).FirstIcon;
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            try
            {
                if (fileDialog.ShowDialog() != true) return;
                var stream = new FileStream(fileDialog.FileName, FileMode.Open);
                icon.FromStream(stream);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка чтения файла {fileDialog.FileName}", "Ошибка чтения", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            

        }
    }
}
