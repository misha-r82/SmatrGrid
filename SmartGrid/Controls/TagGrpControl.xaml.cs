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

        private void CommandBold_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<Tag>().ToArray();
            if(!tags.Any()) return;
            var value = !tags.First().Header.Style.Bold;
            foreach (Tag tw in tags) tw.Header.Style.Bold = value;
        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<Tag>().ToArray();
            if (!tags.Any()) return;
            var value = !tags.First().Header.Style.Italic;
            foreach (Tag tw in tags) tw.Header.Style.Italic = value;
        }

        private void CommandUnderline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<Tag>().ToArray();
            if (!tags.Any()) return;
            var value = !tags.First().Header.Style.Underline;
            foreach (Tag tw in tags) tw.Header.Style.Underline = value;
        }
    }
}
