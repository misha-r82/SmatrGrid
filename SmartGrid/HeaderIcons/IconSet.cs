using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartGrid.HeaderIcons
{
    public class IconSet : ObservableCollection<HeaderIcon>
    {
        private IconSet() { }
        public IconSet(IEnumerable<HeaderIcon> source = null)
        {
            if (source!=null)
                foreach (var item in source)
                    Add(item);
        }
        private bool SameCategory(HeaderIcon icon, HeaderIcon icon1)
        {
            return icon.Collection.Contains(icon1) || icon1.Collection.Contains(icon);
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

    }
}