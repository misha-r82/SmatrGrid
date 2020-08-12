using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lib;
using SmartGrid.Drag;
using SmartGrid.Undo;


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

        private static string GetOperaionName(SwapMode mode)
        {
            switch (mode)
            {
                case SwapMode.Copy: return "копировать";
                case SwapMode.Swap: return "поменять местами";
                default: return "перемещение";
            }
        }

        private static void CreateUndoScope(DragData data)

        {


        }
        public static void DoDrag(DragData data)
        {
            var name =
                $"{GetOperaionName(data.Mode)} {data.@from.FirstElement.Header.Header} -> {data.to.Container.Header.Header}";
            var undoChain = new UndoChain(name);
            undoChain.Add(UndoCollectionScopeFactory.CreateScope(data.@from.Container));
            undoChain.Add(UndoCollectionScopeFactory.CreateScope(data.to.Container));
            switch (data.Mode)
            {
                case SwapMode.Copy:
                {
                    var clone = data.@from.Elements.Select(e => e.GetClone());
                    data.to.Add(clone);
                    break;
                }
                case SwapMode.Replace:
                {
                    data.@from.Remove(data.from.Elements);
                    data.to.Add(data.@from.Elements);
                    break;
                }
                case SwapMode.Swap:
                {
                    if (data.to.FirstElement.GetType()!= typeof(Tag) || data.to.Container.GetType() != typeof(SmartFiled))
                        data.to.Remove(data.to.Elements);
                    if (data.@from.FirstElement.GetType() != typeof(Tag) || data.@from.Container.GetType() != typeof(SmartFiled))
                            data.from.Remove(data.@from.Elements);
                    data.to.Add(data.@from.Elements);
                    data.@from.Add(data.to.Elements);
                    break;
                }
            }
            WorkSpace.Instance.Undo.AddScope(undoChain);
        }
        public static void DoDrag(object sender, DragEventArgs e)
        {
            var dragData = new DragData(sender, e);
            DoDrag(dragData);
        }
    }
}
