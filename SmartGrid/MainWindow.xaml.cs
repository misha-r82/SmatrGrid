using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using SmartGrid.HeaderIcons;
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

        private void Tag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            Tag tag = element.DataContext as Tag;
            if (tag == null) return;
            var dragElement = new DragProcessor.DragElement((DragProcessor.IDragElement)tag, WorkSpace.Instance.ActiveField);
            DragHelper.SetClick(dragElement, e);
        }

        private void btnIconEditor_Click(object sender, RoutedEventArgs e)
        {
            var f = new FrmIconEditor();
            if (f.ShowDialog() != true) ;
            //TODO: UndoEdition
        }

        private void GridMain_OnGotFocus(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.Curent.GotFocuse(e);
        }
    }

}

