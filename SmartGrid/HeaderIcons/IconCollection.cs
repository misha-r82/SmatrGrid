using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartGrid.HeaderIcons
{
    public class IconCollection : ObservableCollection<HeaderIcon>
    {
        private bool SameCategory(HeaderIcon icon, HeaderIcon icon1)
        {
            return icon.IconCollection.Contains(icon1) || icon1.IconCollection.Contains(icon);
        }
        public void Add(HeaderIcon icon)
        {
            if (Contains(icon)) return;
            foreach (HeaderIcon present in this)
            {
                if (SameCategory(present, icon))
                {
                    int pos = IndexOf(present);
                    Remove(present);
                    Insert(pos, icon);
                    return;
                }
            }
            base.Add(icon);
        }
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