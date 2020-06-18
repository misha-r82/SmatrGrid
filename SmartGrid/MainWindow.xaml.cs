using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SmartGrid.Drag;
using SmartGrid.Undo;

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            WorkSpace.Instance = new WorkSpace();
            WorkSpace.Instance.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "ActiveField") SetTagsDataContext();
            };
            InitializeComponent();
            SetTagsDataContext();
        }

        private void SetTagsDataContext()
        {
            var space = WorkSpace.Instance;
            DataContext = space;
            if (space.ActiveField == null) return;
            tag11.DataContext = space.ActiveField.Cells[0, 0];
            tag12.DataContext = space.ActiveField.Cells[0, 1];
            tag13.DataContext = space.ActiveField.Cells[0, 2];
            tag21.DataContext = space.ActiveField.Cells[1, 0];
            tag22.DataContext = space.ActiveField.Cells[1, 1];
            tag23.DataContext = space.ActiveField.Cells[1, 2];
            tag31.DataContext = space.ActiveField.Cells[2, 0];
            tag32.DataContext = space.ActiveField.Cells[2, 1];
            tag33.DataContext = space.ActiveField.Cells[2, 2];
        }
        private void SpltH1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.H1 + WorkSpace.Instance.ActiveField.GridWidth.H2;
            WorkSpace.Instance.ActiveField.GridWidth.H1 = WorkSpace.Instance.ActiveField.GridWidth.H2 = all/2;
        }

        private void SpltH2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.H2 + WorkSpace.Instance.ActiveField.GridWidth.H3;
            WorkSpace.Instance.ActiveField.GridWidth.H2 = WorkSpace.Instance.ActiveField.GridWidth.H3 = all / 2;
        }

        private void SpltW1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.W1 + WorkSpace.Instance.ActiveField.GridWidth.W2;
            WorkSpace.Instance.ActiveField.GridWidth.W1 = WorkSpace.Instance.ActiveField.GridWidth.W2 = all / 2;
        }

        private void SpltW2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var all = WorkSpace.Instance.ActiveField.GridWidth.W2 + WorkSpace.Instance.ActiveField.GridWidth.W3;
            WorkSpace.Instance.ActiveField.GridWidth.W2 = WorkSpace.Instance.ActiveField.GridWidth.W3 = all / 2;
        }

        private void BtnLoad_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Таблицы (*.grd)|*.grd";
            if (fileDialog.ShowDialog(this) != true) return;
            WorkSpace.Instance = Lib.FileIO.DeserializeDataContract<WorkSpace>(fileDialog.FileName);
            WorkSpace.Instance.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == "ActiveField") SetTagsDataContext();
            };
            SetTagsDataContext();
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {

            var fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Таблицы (*.grd)|*.grd";
            if (fileDialog.ShowDialog(this) != true) return;
            Lib.FileIO.SerializeDataContract(WorkSpace.Instance, fileDialog.FileName);
        }

        private void GridMain_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            DragHelper.Track(e);
        }
        private void CommandUndo_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.Undo();
        }
        private void CommandRedo_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.Redo();
        }

        private void CommandUndo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WorkSpace.Instance.Undo.CanUndo;
        }

        private void CommandRedo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WorkSpace.Instance.Undo.CanRedo;
        }

        private void RedoToItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var redoItem = ((FrameworkElement)sender).DataContext as UndoScope;
            WorkSpace.Instance.Undo.RedoToScope(redoItem);
        }

        private void UndoToItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             var undoItem = ((FrameworkElement) sender).DataContext as UndoScope;   
             WorkSpace.Instance.Undo.UndoToScope(undoItem);
        }
    }

}

