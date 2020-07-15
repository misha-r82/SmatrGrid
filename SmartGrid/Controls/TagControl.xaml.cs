using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;
using Lib;
using SmartGrid.Annotations;
using SmartGrid.Controls;
using SmartGrid.Drag;
using SmartGrid.Undo;

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    public partial class TagControl : UserControl, INotifyPropertyChanged
    {
        public TagControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                CurTag = DataContext as TagWrap;
                NewNode = new Node();
                gridNewNode.DataContext = NewNode;
            };
            panelTools.DataContext = this;

        }
        public Node NewNode { get; private set; }
        private Node _droppedNode;
        private HeaderUndoScope<Tag> _headerUndoScope;
        private Node[] SelectedNodes
        {
            get { return lstMain.SelectedItems.OfType<Node>().ToArray(); }
        }
        private TagWrap CurTag { get; set; }
        private void AddNew()
        {
            var tmp = txtNewNode.DataContext;
            CurTag.Tag.Add(NewNode.GetClone());
            NewNode.Header.Header = "";
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            AddNew();
        }
        private void txtNewNode_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!Sett.Settings.CtrlEnter) AddNew();
                else
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
                    AddNew();                               
            }
        }
        private void Grid_Drop(object sender, DragEventArgs e)
        {
            DragProcessor.DoDrag(sender, e);
        }
        // нода
        private void Node_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var nodeCtrl = sender as NodeCotrol;
            if (nodeCtrl != null && nodeCtrl.IsDraEnable)
            {
                var nodes = new List<Node>(SelectedNodes);
                var node = ((FrameworkElement) sender).DataContext as Node;
                if (!nodes.Contains(node)) nodes.Add(node);
                var data = new DragProcessor.DragElement( nodes, CurTag);
                DragHelper.SetClick(data, e);
            }

            e.Handled = false;
        }
        //тэг
        private void CanvTag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!ctrlHeader.IsEditing)
            {
                var element = (FrameworkElement)sender;
                TagWrap tag = element.DataContext as TagWrap;
                if (tag == null) return;
                var dragElement = new DragProcessor.DragElement(tag, tag);
                DragHelper.SetClick(dragElement, e);
            }

            e.Handled = false;
        }
        private void CommDell_Exec(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.Remove(SelectedNodes);
        }


        private void BtnLink_OnClick(object sender, RoutedEventArgs e)
        {
            Editor.NodeEditor.HyperlinkCmd();
        }

        private void CopyNodesToClipboard(IEnumerable<Node> nodes)
        {
            Editor.NodeEditor.SaveToNodeVal();
            var serialized = FileIO.serializeXML(nodes);
            Clipboard.SetData(DragProcessor.DargContentType.Nodes.ToString(), serialized);
        }
        private void CommandCopy_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(SelectedNodes);
        }

        private void CommandCut_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var selected = SelectedNodes.ToArray();
            CurTag.Tag.Remove(selected);
            CopyNodesToClipboard(selected);
        }

        private void CommandPaste_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Nodes.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Nodes.ToString()).ToString();
                var nodes = FileIO.deserializeXMLFromString<IEnumerable<Node>>(serialized);
                if (nodes != null)
                {
                    foreach (Node node in nodes)
                        node.ViewStl.DetailsVisile = false;
                    CurTag.Tag.Add(nodes);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CommandBold_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var nodes = lstMain.SelectedItems.OfType<Node>().ToArray();
            if (!nodes.Any()) return;
            var value = !nodes.First().Header.Style.Bold;
            foreach (Node node in nodes) node.Header.Style.Bold = value;
        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var nodes = lstMain.SelectedItems.OfType<Node>().ToArray();
            if (!nodes.Any()) return;
            var value = !nodes.First().Header.Style.Italic;
            foreach (Node node in nodes) node.Header.Style.Italic = value;
        }

        private void CommandUnderline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var nodes = lstMain.SelectedItems.OfType<Node>().ToArray();
            if (!nodes.Any()) return;
            var value = !nodes.First().Header.Style.Underline;
            foreach (Node node in nodes) node.Header.Style.Underline = value;
        }

        private void CommandBoldTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Header.Style.Bold = !CurTag.Header.Style.Bold;
        }

        private void CommandItalicTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Header.Style.Italic = !CurTag.Header.Style.Italic;
        }

        private void CommandBindingUndelineTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Header.Style.Underline = !CurTag.Header.Style.Underline;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
           // Debug.WriteLine($"GotFocus{CurTag.Header.Header}");
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"LostFocus{CurTag.Header.Header}");
        }


        private void CtrlHeader_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _headerUndoScope = new HeaderUndoScope<Tag>(CurTag.Tag, "Изменение заголовка набора {0}");
        }

        private void CtrlHeader_OnLostFocus(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.AddScope(_headerUndoScope);
        }
    }
}
