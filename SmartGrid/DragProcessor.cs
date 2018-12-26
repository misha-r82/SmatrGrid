using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lib;

namespace SmartGrid
{
    public class DragProcessor
    {
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

        public static void DragNodes(TagWrap curTag, DragEventArgs e, Node insertBefore = null)
        {
            var data = (KVPair<TagWrap, Node[]>)e.Data.GetData(typeof(KVPair<TagWrap, Node[]>));
            if (data == null || !data.Val.Any()) return;
            var tagFrom = data.Key;

            var mode = GetDragMode(e);
            if (mode == SwapMode.Copy)
                curTag.Tag.Add(data.Val, insertBefore, true);
            else
            {
                curTag.Tag.Add(data.Val, insertBefore, false);
                if (tagFrom != curTag)
                    tagFrom.Tag.Remove(data.Val);
            }
        }
    }
}
