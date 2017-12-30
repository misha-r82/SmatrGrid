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
        private Tag CurTag { get { return lstMain.DataContext as Tag; } }
        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var headNode = element.DataContext as HeadNode;
            headNode.AddToTag();
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            var data = (Node[])e.Data.GetData(typeof(Node[]));
            if (!data.Any()) return;
            var tag = data.First().Tag;
            if (tag == CurTag) return;
            if (CopyMode)
            {
                var clones = data.Select(d => d.GetClone());
                foreach (Node clone in clones)
                    clone.Tag = CurTag;
                CurTag.Add(data);
            }
            else
            {
                var oldTag = data.First().Tag;
                foreach (Node node in data)
                    node.Tag = CurTag;
                CurTag.Add(data);
                oldTag.Remove(data);
            }

            
        }

        private void Drag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var node = (Node) element.DataContext;
            var data = SelectedNodes.ToList();
            if (!data.Any(d=>d.Header.Equals(node.Header, StringComparison.OrdinalIgnoreCase)))
                data.Add(node);
            //if (data.Length == 0) data = new[] {};
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
                DragDrop.DoDragDrop(element, data.ToArray(), dragEfect);
            e.Handled = false;
        }


        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            CurTag.Remove(SelectedNodes);
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
