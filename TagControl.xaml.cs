using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }
        private bool CopyMode { get { return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); } }
        private Node[] SelectedNodes
        {
            get { return lstMain.SelectedItems.OfType<Node>().ToArray(); }
        }
        private TagWrap CurTag { get { return lstMain.DataContext as TagWrap; } }
        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var headNode = element.DataContext as HeadNode;
            headNode.AddToTag();
        }

        private void NodeList_Drop(object sender, DragEventArgs e)
        {
            var data = (Node[])e.Data.GetData(typeof(Node[]));
            if (data == null || !data.Any()) return;
            var tag = data.First().Tag;
            if (tag == CurTag) return;
            if (CopyMode)
            {
                var clones = data.Select(d => d.GetClone());
                foreach (Node clone in clones)
                    clone.Tag = CurTag;
                CurTag.Tag.Add(data);
            }
            else
            {
                var oldTag = data.First().Tag;
                foreach (Node node in data)
                    node.Tag = CurTag;
                CurTag.Tag.Add(data);
                oldTag.Tag.Remove(data);
            }

            
        }

        private void Drag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var node = element.DataContext as Node;
            if (node == null) return;
            var data = SelectedNodes.ToList();
            if (!data.Any(d => d.Header.Equals(node.Header, StringComparison.OrdinalIgnoreCase)))
                data.Add(node);
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
            DragDrop.DoDragDrop(element, data.ToArray(), dragEfect);
            e.Handled = false;
        }

        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            CurTag.Tag.Remove(SelectedNodes);
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CanvTag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var tag = element.DataContext as TagWrap;
            if (tag == null) return;
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
            DragDrop.DoDragDrop(element, tag, dragEfect);
            e.Handled = false;
        }

        private void Tag_Drop(object sender, DragEventArgs e)
        {
            var second = e.Data.GetData(typeof(TagWrap)) as TagWrap;
            if (second == null) return;
            CurTag.SwapWith(second);
        }
    }
}
