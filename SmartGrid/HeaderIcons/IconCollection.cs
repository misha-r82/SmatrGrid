using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SmartGrid.HeaderIcons
{
    public class IconCollection : ObservableCollection<IconElement>
    {
        private IconCollection():base()
        { }
        public IconCollection(string name, IconsGroup group) : base()
        {
            CreatElement(name);
        }
        public IconElement FirstIcon => this[0];

        public IconElement CreatElement(string name, Stream stream = null)
        {
            var element = new IconElement(name, this);
            element.FromStream(stream);
            Add(element);
            return element;
        }
        public IconElement NextIcon(IconElement icon)
        {
            int pos = IndexOf(icon);
            if(pos < 0) throw new ArgumentException("icon is not contains in Collection");
            if (pos == Count - 1) return this[0];
            return this[pos+1];
        }
    }

}