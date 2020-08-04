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
                _mode = GetDragMode(e);
                var senderElement = ((FrameworkElement) sender).DataContext as IDragElement;
                var conteiner = ((FrameworkElement) e.OriginalSource).DataContext as IDragElement;
                if (conteiner is IContainer && ((IContainer) conteiner).AcceptType(senderElement.GetType()))
                {
                    to = new DragElement(senderElement, conteiner);
                }
                else
                {
                    
                    to = new DragElement(conteiner, senderElement);
                }
                
            }
        }
    }
}