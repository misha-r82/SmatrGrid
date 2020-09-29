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
        static RoutedUICommand _addCommand = new RoutedUICommand("Добавить изображение", "AddIcon",typeof(IconEditorCommands));
        public static RoutedUICommand AddIcon
        {
            get => _deleteCommand;
        }
        static RoutedUICommand _deleteCommand = new RoutedUICommand("Удалить", "DeleteIcons", typeof(IconEditorCommands));
        public static RoutedUICommand DeleteIcons
        {
            get => _deleteCommand;
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



        private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.CoreHeaderIcon.IconCollection.Remove(iconTree.SelectedIcon);
        }        
        public HeaderIcon SelectedItem { get; private set; }
        public HeaderIcon ParenItem { get; private set; }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CommandAdd_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() != true) return;
            foreach (var fileName in fileDialog.FileNames)
            {
                if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                string name = Path.GetFileNameWithoutExtension(fileName);
                var stream = new FileStream(fileName, FileMode.Open);
                SelectedItem.IconCollection.Add(new HeaderIcon(stream) { Name = name });
            }
        }

        private void CommandDelete_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ParenItem == null || SelectedItem == null) return;
            ParenItem.IconCollection.Remove(SelectedItem);
        }

        private void ctrlMainIcon_GotFocus(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            SelectedItem = element.DataContext as HeaderIcon;
            var parentCtrl = Lib.VisualTreeHelpers.FindAncestor<CtrlIconEdition>(element);
            if (parentCtrl == null) return;
            ParenItem = parentCtrl.DataContext as HeaderIcon;
        }
    }
}
