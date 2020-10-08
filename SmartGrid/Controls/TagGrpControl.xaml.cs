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
using SmartGrid.Controls;
using SmartGrid.Drag;
using static SmartGrid.DragProcessor;

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    public partial class TagGrpControl : UserControl
    {
        public TagGrpControl()
        {
            InitializeComponent();
        }
        private TagGroup TagGrp { get { return DataContext as TagGroup;} }
        private Tag[] SelectedTags
        {
            get { return lstMain.SelectedItems.OfType<Tag>().ToArray(); }
        }
        private void List_Drop(object sender, DragEventArgs e)
        {
             DoDrag(sender, e);         
        }
        private void CanvTag_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = (FrameworkElement) sender;
            var tag = item.DataContext as Tag;
            if (tag == null) return;
            DragHelper.SetClick(new DragElement((IDragElement)tag, TagGrp), e);
            e.Handled = false;           
        }
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (Tag tag in SelectedTags)
                TagGrp.Remove(tag);
        }

        private void LstMain_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkSpace.Instance.Curent.SetSelectedElements(lstMain.SelectedItems);
        }
    }
}
