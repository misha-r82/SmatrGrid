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



        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            DragDrop.DoDragDrop(btn, "123", DragDropEffects.Copy);  
        }
        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            var data = (Node[])e.Data.GetData(typeof(Node[]));
            CurTag.Add(data);
        }

        private void Drag_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var data = SelectedNodes;
            //if (data.Length == 0) data = new[] {(Node) element.DataContext};
                DragDrop.DoDragDrop(element, data, DragDropEffects.Move);
            e.Handled = false;
        }


        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            CurTag.Remove(SelectedNodes);
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            txtVal.
        }
    }
}
