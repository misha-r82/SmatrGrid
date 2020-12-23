using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartGrid.HeaderIcons
{
    public class IconSet : ObservableCollection<IconElement>
    {
        private IconSet() { }
        public IconSet(IEnumerable<IconElement> source = null)
        {
            if (source!=null)
                foreach (var item in source)
                    Add(item);
        }
        private bool SameCategory(IconElement icon, IconElement icon1)
        {
            return icon.Collection == icon1.Collection;
        }
        public void Add(IconElement icon)
        {
            if (Contains(icon)) return;
            foreach (IconElement present in this)
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

        public void NextIcon(IconElement icon)
        {
            if (!Contains(icon)) throw new ArgumentException("Cannot set NextItem beacuse icon is noi in the set");
            var pos = IndexOf(icon);
            RemoveAt(pos);
            Insert(pos, icon.Collection.NextIcon(icon));
        }

    }
}