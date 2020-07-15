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
 
        public static void DoDrag(DragData data)
        {
            data.to.Add(data.@from.Elements);
            switch (data.Mode)
            {
                case SwapMode.Replace:
                {
                    data.@from.Remove(data.from.Elements);
                    break;
                }
                case SwapMode.Swap:
                {
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
