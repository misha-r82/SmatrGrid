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

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    public partial class TagControl : UserControl
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

        }
        public Node NewNode { get; private set; }
        private Node _droppedNode;
        private bool CopyMode { get { return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); } }
        private Point _prewPt;
        private Node[] SelectedNodes
        {
            get { return lstMain.SelectedItems.OfType<Node>().ToArray(); }
        }

        private void AddNew()
        {
            CurTag.Tag.Add(NewNode.GetClone());
            NewNode.Header = "";
            NewNode.Val = "";           
        }
        private TagWrap CurTag { get; set; }
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
        private void NodeList_Drop(object sender, DragEventArgs e)
        {
            var pt = e.GetPosition(this);
            if (Math.Abs(pt.Y - _prewPt.Y) < 10) return;
            var secondTag = e.Data.GetData(typeof(TagWrap)) as TagWrap;
            var mode = DragProcessor.GetDragMode(e);
            if (secondTag != null) CurTag.SwapWith(secondTag, mode);
            DragProcessor.DragNodes(CurTag, e, _droppedNode);
        }
        // нода
        private void Drag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _prewPt = e.GetPosition(this);
            var element = (FrameworkElement)sender;
            var node = element.DataContext as Node;
            if (node == null) return;
            var data = SelectedNodes.ToList();
            if (!data.Any(d => d.Header.Equals(node.Header, StringComparison.OrdinalIgnoreCase)))
                data.Add(node);
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
            var nodesPack = new KVPair<TagWrap, Node[]>(CurTag, data.ToArray());
            DragDrop.DoDragDrop(element, nodesPack, dragEfect);
            e.Handled = false;
        }
        //тэг
        private void CanvTag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var tag = element.DataContext as TagWrap;
            if (tag == null) return;
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
            DragDrop.DoDragDrop(element, tag, dragEfect);
            e.Handled = false;
        }
        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void Node_OnDrop(object sender, DragEventArgs e)
        {
            var node = ((FrameworkElement) sender).DataContext as Node;
            if (node == null) return;
            _droppedNode = node;
        }
        private void CommDell_Exec(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Tag.Remove(SelectedNodes);
        }

    }
}
