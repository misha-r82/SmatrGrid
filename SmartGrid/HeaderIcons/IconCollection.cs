using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SmartGrid.HeaderIcons
{
    public class IconCollection : ObservableCollection<HeaderIcon>
    {
        private IconCollection():base()
        { }
        public IconCollection(HeaderIcon firstIcon) : base()
        {
            Add(firstIcon);
        }
        public HeaderIcon NextIcon(HeaderIcon icon)
        {
            int pos = IndexOf(icon);
            if(pos < 0) throw new ArgumentException("icon is not contains in Collection");
            if (pos == Count - 1) return this[0];
            return this[pos+1];
        }
    }

}