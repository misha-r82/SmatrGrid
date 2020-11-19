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
using Lib;
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
        private void CopyNodesToClipboard(IEnumerable<IHasHeader> elements)
        {
            if (elements == null || !elements.Any()) return;
            var arr = elements.ToArray();
            Editor.NodeEditor.SaveToNodeVal();
            string serialized = "";
            if (arr[0] is Node)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<Node>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Nodes.ToString(), serialized);
            }
            else
            if (arr[0] is Tag)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<Tag>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Tag.ToString(), serialized);
            }
            else if (arr[0] is SmartFiled)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<SmartFiled>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Field.ToString(), serialized);
            }

        }
        private void CommandCopy_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandCut_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
            var contayiner = WorkSpace.Instance.Curent.Contayner as DragProcessor.IContainer;
            contayiner?.Remove(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandPaste_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IHasHeader[] elements = null;
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Nodes.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Nodes.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<Node[]>(serialized).Cast<IHasHeader>().ToArray();
            }
            else
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Tag.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Tag.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<Tag[]>(serialized).Cast<IHasHeader>().ToArray();
            }
            else if (Clipboard.ContainsData(DragProcessor.DargContentType.Field.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Field.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<SmartFiled[]>(serialized).Cast<IHasHeader>().ToArray();
            }
            var contayiner = WorkSpace.Instance.Curent.Contayner;
            contayiner?.Add(elements);
        }

        private void CommandUndo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WorkSpace.Instance.Undo.CanUndo;
        }

        private void CommandRedo_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = WorkSpace.Instance.Undo.CanRedo;
        }
        private void CommandBold_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Bold = !element.Header.Style.Bold;

        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Italic = !element.Header.Style.Italic;
        }

        private void CommandUndeline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var element in WorkSpace.Instance.Curent.SelectedElements)
                element.Header.Style.Underline = !element.Header.Style.Underline;
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

        private void On_Drop(object sender, DragEventArgs e)
        {
            DragProcessor.DoDrag(ctrlField, e);
        }

        private void Tag00_OnDrop(object sender, DragEventArgs e)
        {
            DragProcessor.DoDrag(this, e);
        }
    }

}

