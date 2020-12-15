using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SmartGrid.HeaderIcons
{
    public class IconCollection : ObservableCollection<IconElement>
    {
        private IconCollection():base()
        { }
        public IconCollection(IconElement firstIcon) : base()
        {
            Add(firstIcon);
        }
        public IconCollection(IEnumerable<IconElement> sourceCollection) : base(sourceCollection) {}
        /*public HeaderIcon NextIcon(HeaderIcon icon)
        {
            var thisIcon = WithSameIconData(icon);
            int pos = IndexOf(thisIcon);
            if(pos < 0) throw new ArgumentException("icon is not contains in Collection");
            if (pos == Count - 1) return this[0];
            return this[pos+1];
        }*/
    }

}