using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.HeaderIcons
{
    public class IconsGroup : ObservableCollection<IconCollection>
    {
        public IconsGroup() : base(){}

        public IconCollection CreateCollection(string name)
        {
            var collection = new IconCollection(name,this);
            Add(collection);
            return collection;
        }
    }
}
