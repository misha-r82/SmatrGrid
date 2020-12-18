using System;
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
using Path = System.IO.Path;

namespace SmartGrid.HeaderIcons
{
    public partial class FrmIconEditor : Window
    {
        private IconElement selectedItem;
        private IconCollection selectedCollection;

        public FrmIconEditor()
        {
            DataContext = WorkSpace.Instance.IconGroup;
            InitializeComponent();
            SelectedCollection = WorkSpace.Instance.IconGroup[0];
        }

        public IconElement SelectedItem
        {
            get => selectedItem;
            private set
            {
                if (value == selectedItem) return;
                selectedItem = value;
                IconEditorCommands.AddIconComamnd.SelectedIcon = value;
            }
        }

        public IconCollection SelectedCollection
        {
            get => selectedCollection;
            private set
            {
                if (SelectedCollection == value) return;
                selectedCollection = value;
                IconEditorCommands.AddCollection.Collection = value;
                IconEditorCommands.AddIconComamnd.Collection = value;
            }
        }

        private void CommandDelete_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectedItem == null) return;
            SelectedItem.Collection.Remove(SelectedItem);
            
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
