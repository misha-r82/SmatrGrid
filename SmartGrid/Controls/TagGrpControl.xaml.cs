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
        private TagWrap[] SelectedTags
        {
            get { return lstMain.SelectedItems.OfType<TagWrap>().ToArray(); }
        }
        private void List_Drop(object sender, DragEventArgs e)
        {
             DragProcessor.DoDrag(sender, e);         
        }
        private void CanvTag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = (FrameworkElement) sender;
            var tagWrp = item.DataContext as TagWrap;
            if (tagWrp == null) return;
            DragHelper.SetClick(new DragProcessor.DragContent(tagWrp) {Group = TagGrp}, e);
            e.Handled = false;           
        }
        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (TagWrap tag in SelectedTags)
                TagGrp.Remove(tag);
        }

        private void CommandBold_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<TagWrap>().ToArray();
            if(!tags.Any()) return;
            var value = !tags.First().HeaderStyle.Bold;
            foreach (TagWrap tag in tags) tag.HeaderStyle.Bold = value;
        }

        private void CommandItalic_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<TagWrap>().ToArray();
            if (!tags.Any()) return;
            var value = !tags.First().HeaderStyle.Italic;
            foreach (TagWrap tag in tags) tag.HeaderStyle.Italic = value;
        }

        private void CommandUnderline_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tags = lstMain.SelectedItems.OfType<TagWrap>().ToArray();
            if (!tags.Any()) return;
            var value = !tags.First().HeaderStyle.Underline;
            foreach (TagWrap tag in tags) tag.HeaderStyle.Underline = value;
        }
    }
}
