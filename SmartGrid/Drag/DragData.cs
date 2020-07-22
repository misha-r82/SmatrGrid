using System;
using System.Windows;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public class DragData
        {
            public DragElement from;
            public DragElement to;
            private DragProcessor.SwapMode _mode;
            public SwapMode Mode => _mode;
            public DragData(object sender, DragEventArgs e)
            {
                from = e.Data.GetData(typeof(DragElement)) as DragElement;
                if (from == null) throw new ArgumentException("Drag incorect type from");
                _mode = GetDragMode(e);
                var senderElement = ((FrameworkElement) sender).DataContext as IHasHeader;
                var conteiner = ((FrameworkElement) e.OriginalSource).DataContext as IHasHeader;
                if (conteiner.GetType() == @from.FirstElement.GetType())
                {
                    to = new DragElement(conteiner,senderElement);
                }
                else
                {
                    to = new DragElement(senderElement, conteiner);
                }
                
            }
        }
    }
}