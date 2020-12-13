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
            return icon.Collection.Any(i=>i.IsSameIconData(icon1));
        }
        public void Add(HeaderIcon icon)
        {
            if (this.Any(i => i.Icon == icon.Icon)) return;
            foreach (HeaderIcon present in this)
            {
                if (SameCategory(present, icon))
                {
                    int pos = IndexOf(present);
                    Remove(present);
                    Insert(pos, icon.ToIconSet());
                    return;
                }
            }
            base.Add(icon.ToIconSet());
        }

        public void NextIcon(HeaderIcon icon)
        {
            if (!Contains(icon)) throw new ArgumentException("Cannot set NextItem beacuse icon is noi in the set");
            var pos = IndexOf(icon);
            RemoveAt(pos);
            Insert(pos, icon.NextIcon());
        }

    }
}