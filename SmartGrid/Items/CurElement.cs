using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartGrid.Controls;
using static SmartGrid.DragProcessor;

namespace SmartGrid.Items
{
    public class CurElement
    {
        public IContainer Contayner { get; private set; }
        public IHasHeader[] SelectedElements { get; private set; }

        public void SetSelectedElements(IList items)
        {
            if (items.Count < 1) return;
            //Debug.WriteLine(((IHasHeader)items[0]).Header.Header + " Selected");
            SelectedElements = items.OfType<IHasHeader>().ToArray();
        }
        public void GotFocuse(RoutedEventArgs e)
        {
            var element = (e.OriginalSource as FrameworkElement)?.DataContext as IHasHeader;
            if (element == null) return;
            var contaynerElement = (e.Source as FrameworkElement).DataContext as IContainer;
            if (contaynerElement == null) return;
            SelectedElements = new[] {element};
            Contayner = contaynerElement;
        }
    }
}
