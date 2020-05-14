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
using SmartGrid.Drag;

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
        private bool _isNodeEditionMode;
        private Node[] SelectedNodes
        {
            get { return lstMain.SelectedItems.OfType<Node>().ToArray(); }
        }
        private TagWrap CurTag { get; set; }
        private void AddNew()
        {
            CurTag.Tag.Add(NewNode.GetClone());
            NewNode.Header = "";
            NewNode.Val = "";           
        }
        public bool IsNodeEditionMode
        {
            get { return _isNodeEditionMode; }
            set
            {
                //if (_contentLoaded == value) return;
                _isNodeEditionMode = value;
                OnPropertyChanged();
            }
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
            
            var nodes = new List<Node>(SelectedNodes);
            var node = ((FrameworkElement) sender).DataContext as Node;
            if (!nodes.Contains(node)) nodes.Add(node);
            DragHelper.SetClick(new DragProcessor.DragContent(nodes, CurTag), e);
            e.Handled = false;
        }
        //тэг
        private void CanvTag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var tag = element.DataContext as TagWrap;
            if (tag == null) return;
            DragHelper.SetClick(new DragProcessor.DragContent(tag), e);
            e.Handled = false;
        }
        private void CommDell_Exec(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.Remove(SelectedNodes);
        }

        private void SetEditorRtb(object rtb)
        {
            var tb = rtb as RichTextBox;
            if (tb == null) return;
            Editor.NodeEditor.Rtb = tb;
        }
        private void TxtVal_OnGotFocus(object sender, RoutedEventArgs e)
        {
            SetEditorRtb(sender);
            IsNodeEditionMode = true;            
        }
        private void TxtVal_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IsNodeEditionMode = false;
            Editor.NodeEditor.SaveToNodeVal();
        }
        private void BtnLink_OnClick(object sender, RoutedEventArgs e)
        {
            Editor.NodeEditor.HyperlinkCmd();
        }
        private void TxtVal_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetEditorRtb(sender);
        }
        private void TxtVal_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var tb = sender as RichTextBox;
            if (tb == null) return;
            tb.Document.PageWidth = tb.ActualWidth;
        }
        private void FrameworkElement_OnInitialized(object sender, EventArgs e)
        {
            SetEditorRtb(sender);
        }
        private void BtnCut_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNodeEditionMode)
                Editor.NodeEditor.Rtb.Cut();
            else
            {
                var selected = SelectedNodes.ToArray();
                CurTag.Tag.Remove(selected);
                CopyNodesToClipboard(selected);
            }
        }

        private void CopyNodesToClipboard(IEnumerable<Node> nodes)
        {
            Editor.NodeEditor.SaveToNodeVal();
            var serialized = FileIO.serializeXML(nodes);
            Clipboard.SetData(DragProcessor.DargContentType.Nodes.ToString(), serialized);
        }
        private void BtnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNodeEditionMode)
                Editor.NodeEditor.Rtb.Copy();            
           else CopyNodesToClipboard(SelectedNodes);
        }
        private void BtnPaste_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsNodeEditionMode)
                Editor.NodeEditor.Rtb.Paste();
            else
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
            var value = !nodes.First().HeaderStyle.Bold;
            foreach (Node node in nodes) node.HeaderStyle.Bold = value;
        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var nodes = lstMain.SelectedItems.OfType<Node>().ToArray();
            if (!nodes.Any()) return;
            var value = !nodes.First().HeaderStyle.Italic;
            foreach (Node node in nodes) node.HeaderStyle.Italic = value;
        }

        private void CommandUnderline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var nodes = lstMain.SelectedItems.OfType<Node>().ToArray();
            if (!nodes.Any()) return;
            var value = !nodes.First().HeaderStyle.Underline;
            foreach (Node node in nodes) node.HeaderStyle.Underline = value;
        }

        private void CommandBoldTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.HeaderStyle.Bold = !CurTag.Tag.HeaderStyle.Bold;
        }

        private void CommandItalicTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.HeaderStyle.Italic = !CurTag.Tag.HeaderStyle.Italic;
        }

        private void CommandBindingUndelineTag_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.HeaderStyle.Underline = !CurTag.Tag.HeaderStyle.Underline;
        }

        private void GridMain_OnGotFocus(object sender, RoutedEventArgs e)
        {
            //gridNewNode.Visibility = Visibility.Visible;
        }

        private void GridMain_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //gridNewNode.Visibility = Visibility.Visible;
        }
    }
}
