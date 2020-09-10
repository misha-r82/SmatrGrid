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
using Path = System.Windows.Shapes.Path;

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

        public IEnumerable<HeaderIcon> SelectedItems
        {
            get
            {
                foreach (var item in lstMain.SelectedItems.OfType<HeaderIcon>())
                    yield return item;
            }
        }

        private void CommandAdd_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var icon = DataContext as HeaderIcon;
            if (icon == null) return;
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() != true) return;
            foreach (var fileName in fileDialog.FileNames)
            {
                if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                string name = System.IO.Path.GetFileNameWithoutExtension(fileName);
                var stream = new FileStream(fileName, FileMode.Open);
                icon.IconCollection.Add(new HeaderIcon(stream) { Name = name });
            }
        }

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
