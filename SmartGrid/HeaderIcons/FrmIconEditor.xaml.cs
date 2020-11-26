using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace SmartGrid.HeaderIcons
{
    /// <summary>
    /// Логика взаимодействия для IconEditor.xaml
    /// </summary>

    public static class IconEditorCommands
    {
        static RoutedUICommand _addCommand = new RoutedUICommand("Добавить значек", "AddIcon",typeof(IconEditorCommands));
        static RoutedUICommand _addChieldCommand = new RoutedUICommand("Добавить вложенный значек", "AddIconChield", typeof(IconEditorCommands));
        public static RoutedUICommand AddIcon
        {
            get => _addCommand;
        }
        public static RoutedUICommand AddIconChield
        {
            get => _addChieldCommand;
        }

    }
    public partial class FrmIconEditor : Window
    {
        

        public FrmIconEditor()
        {

            DataContext = WorkSpace.Instance.CoreHeaderIcon;
            InitializeComponent();
            SelectedItem = this.DataContext as HeaderIcon;
        }

        public HeaderIcon SelectedItem { get; private set; }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CommandAdd_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddIcon(SelectedItem.Parent == null ? SelectedItem : SelectedItem.Parent);
        }
        private void CommandAddChield_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddIcon(SelectedItem);
        }
        private void AddIcon(HeaderIcon parentIcon)
        {
            if (parentIcon == null) return;
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() != true) return;
            foreach (var fileName in fileDialog.FileNames)
            {
                if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                string name = Path.GetFileNameWithoutExtension(fileName);
                var stream = new FileStream(fileName, FileMode.Open);
                parentIcon.Create(parentIcon, name, stream);
            }
        }
        private void CommandDelete_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            SelectedItem.Parent.RemoveFromItemCollection(SelectedItem);
            
        }

        private void ctrlMainIcon_GotFocus(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            SelectedItem = element.DataContext as HeaderIcon;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IconRepo.Save();
        }
    }
}
