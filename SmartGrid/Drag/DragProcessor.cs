using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lib;
using SmartGrid.Drag;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public enum DargContentType { Tag, TagToGroup, Nodes, Field}

        public enum SwapMode { Swap, Copy, Replace }
        public static SwapMode GetDragMode(DragEventArgs e)
        {
            var mode = SwapMode.Replace;
            switch (e.KeyStates)
            {
                case DragDropKeyStates.ControlKey: mode = SwapMode.Copy; break;
                case DragDropKeyStates.ShiftKey: mode = SwapMode.Swap; break;
            }
            return mode;
        }

        private static bool SwapTag(DragData data)
        {
            var tagWrap = data.@from.FirstElement as TagWrap;
            if (tagWrap != null) 
            {
                tagWrap.SwapWith(data.to.FirstElement, data.to.Container);
                return true;
            }
            tagWrap = data.to.FirstElement as TagWrap;            
            if (tagWrap != null)
            {
                tagWrap.SwapWith(data.@from.FirstElement, data.@from.Container);
                return true;
            }
                
            return false;
        }

        
        public static void DoDrag(DragData data)
        {
            switch (data.Mode)
            {
                case SwapMode.Copy:
                {
                    data.to.Add(data.@from.Elements);
                    break;
                }
                case SwapMode.Replace:
                {
                    //if (data.to.Container.)

                    data.to.Add(data.@from.Elements);
                    if (data.@from.Container != data.to.Container)
                        data.@from.Remove(data.from.Elements);
                    break;
                }
                case SwapMode.Swap:
                {
                    if (data.@from.Container == data.to.Container) return;
                    if (SwapTag(data)) return;
                    data.to.Add(data.@from.Elements);
                    data.@from.Add(data.to.Elements);
                    data.to.Remove(data.to.Elements);
                    data.from.Remove(data.@from.Elements);
                    break;
                }
                
            }
        }
        
        public static void DoDrag(object sender, DragEventArgs e)
        {
            var dragData = new DragData(sender, e);
            DoDrag(dragData);
            if (dragData.@from.FirstElement.GetType() == typeof(TagWrap) && dragData.Mode == SwapMode.Replace)
            {

            }
            /*
            DragContent data = e.Data.GetData(typeof(DragContent)) as DragContent;
            if (data == null) return;
            var elementTo = sender as FrameworkElement;
            if (elementTo == null) return;
            data.DestField = elementTo.DataContext as SmartFiled;
            data.DestTag = elementTo.DataContext as TagWrap;
            data.DestNode = ((FrameworkElement) e.OriginalSource).DataContext as Node;
            if (data.Group == null) data.Group = elementTo.DataContext as TagGroup;
            
            if (data.DestField != null && data.Type != DargContentType.Field)
                data.DestTag = data.DestField.WorkTag;
            data.Mode = GetDragMode(e);
            switch (data.Type)
            {
                case DargContentType.Tag:
                    DragTag(data); break;
                case DargContentType.Nodes:
                    DragNodes(data); break;
                case DargContentType.Field:
                    DragToField(data); break;
            }*/
        }
    }
}
