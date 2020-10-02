using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartGrid.HeaderIcons
{
    public class IconCollection : ObservableCollection<HeaderIcon>
    {
        public void Next(HeaderIcon icon)
        {
            if (!icon.IconCollection.Any()) return;
            int pos = IndexOf(icon);
            if (pos < 0) throw new ArgumentException("IconCollection не содержит переданный Icon");
            RemoveAt(pos);
            Insert(pos, icon);

        }
    }
}