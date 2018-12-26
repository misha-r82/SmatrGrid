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
    public partial class TagGrpControl : UserControl
    {
        public TagGrpControl()
        {
            InitializeComponent();
        }

        private TagGroup TagGrp { get { return DataContext as TagGroup;} }
        private bool CopyMode { get { return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); } }
        private bool ModeReplace { get { return !Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl)
            && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift);} }
        private bool SwapMode { get { return !CopyMode && !ModeReplace; } }
        private TagWrap DragTo;
        private Point _prewPt;
        private TagWrap[] SelectedTags
        {
            get { return lstMain.SelectedItems.OfType<TagWrap>().ToArray(); }
        }

        private void List_Drop(object sender, DragEventArgs e)
        {
            var pt = e.GetPosition(this);
            if (Math.Abs(pt.Y - _prewPt.Y) < 10) return;
            if (TagGrp == null) return;
            var tagWrap = e.Data.GetData(typeof(TagWrap)) as TagWrap;
            var mode = DragProcessor.GetDragMode(e);
            if (tagWrap != null)
            {
                if (DragTo == null) // перетащили на пустое место
                {
                    TagGrp.TagList.Add(new TagWrap() {Tag = tagWrap.Tag});
                    if (!CopyMode) tagWrap.Tag = new Tag();
                }                 
                else
                {
                    if (SwapMode) tagWrap.SwapWith(DragTo, mode);
                    else
                    {
                        TagWrap newTag = new TagWrap();
                        int pos = TagGrp.TagList.IndexOf(DragTo);
                        if (pos == -1) pos = 0;
                        newTag.SwapWith(tagWrap, mode);
                        TagGrp.TagList.Insert(pos, newTag);
                    }
                }
            }
               
            else
            DragProcessor.DragNodes(DragTo, e);
        }

        private void CanvTag_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _prewPt = e.GetPosition(this);
            var item = sender as FrameworkElement;
            var tagWrp = item.DataContext as TagWrap;
            if (tagWrp == null) return;
            var dragEfect = CopyMode ? DragDropEffects.Copy : DragDropEffects.Move;
            DragDrop.DoDragDrop(item, tagWrp, dragEfect);
            if (ModeReplace)
                TagGrp.Remove(tagWrp);
            e.Handled = false;           
        }

        private void ListItemDrop(object sender, DragEventArgs e)
        {
            DragTo = ((FrameworkElement)sender).DataContext as TagWrap;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (TagWrap tag in SelectedTags)
                TagGrp.Remove(tag);
        }
    }
}
