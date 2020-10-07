using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartGrid.Controls;

namespace SmartGrid.Items
{
    public class CurElement
    {
        public IHasHeader Element { get; private set; }
        public IHasHeader[] SelectedElements { get; private set; }

        public void SetSelectedElements(IList items)
        {
            SelectedElements = items.OfType<IHasHeader>().ToArray();
        }
        public void GotFocuse(RoutedEventArgs e)
        {
            /*
            if (e.Source is IHasSelectedHeaderItems)
            {
                SelectedElements = ((IHasSelectedHeaderItems) e.Source).SelectedItems.ToArray();
                Debug.WriteLine(SelectedElements.Length);
            }
            else
            {
                var tagCtrl = e.Source as TagControl;
                if (tagCtrl == null) return;
                SelectedElements = new[] {tagCtrl.DataContext as Tag};
            }*/
            var element = e.OriginalSource as FrameworkElement;
            var hasHeader = element.DataContext as IHasHeader;
            if (hasHeader == null) return;
            Element = hasHeader;
        }
    }
}
