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
        private static readonly AddIconcommand _addiconcomand = new AddIconcommand();

        /*static RoutedUICommand _addCollectionCommand = new RoutedUICommand("Добавить коллекцию значков", "AddCollection",typeof(IconEditorCommands));
        static RoutedUICommand _addIconCommand = new RoutedUICommand("Добавить значек", "AddIcon", typeof(IconEditorCommands));*/
        private static AddIconcommand Addiconcomand => _addiconcomand;
        private static AddIconcommand Addiconcomand => _addiconcomand;
        private static AddIconcommand AddcollectionCommand => _addiconcomand;

        public class AddCollectionCommand : ICommand
        {
            public bool CanExecute(object parameter)
            {
            }

            private IconCollection _collection;

            public IconCollection Collection
            {
                get => _collection;
                set
                {
                    if (_collection == value) return;
                    _collection = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

            public void Execute(object parameter)
            {
                
            }

            public event EventHandler CanExecuteChanged;
        }
        public class AddIconcommand : ICommand
        {
            private IconCollection _collection;

            public IconCollection Collection
            {
                get => _collection;
                set
                {
                    if (_collection == value) return;
                    _collection = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public bool CanExecute(object parameter)
            {
                return _collection != null;
            }

            public void Execute(object parameter)
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog() != true) return;
                foreach (var fileName in fileDialog.FileNames)
                {
                  if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) continue;
                  string name = Path.GetFileNameWithoutExtension(fileName);
                  var stream = new FileStream(fileName, FileMode.Open);
                  Collection.CreatElement(name, stream);
                }
            }

            public event EventHandler CanExecuteChanged;
        }
        /*public static RoutedUICommand AddCollection
        {
            get => _addCollectionCommand;
        }
        public static RoutedUICommand AddIcon
        {
            get => _addIconCommand;
        }*/

    }
    public partial class FrmIconEditor : Window
    {
        

        public FrmIconEditor()
        {
            DataContext = WorkSpace.Instance.IconGroup;
            InitializeComponent();
            SelectedItem = this.DataContext as IconElement;
        }

        public IconElement SelectedItem { get; private set; }
        public IconCollection SelectedCollection { get; private set; }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CommandAdd_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void AddIcon()
        {

        }
        private void CommandDelete_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            /*if (SelectedItem == null) return;
            SelectedItem.Parent.Collection.Remove(SelectedItem);*/
            
        }

        private void ctrlMainIcon_GotFocus(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            SelectedItem = element.DataContext as IconElement;
            SelectedCollection = element.DataContext as IconCollection;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IconRepo.Save();
        }
    }
}
