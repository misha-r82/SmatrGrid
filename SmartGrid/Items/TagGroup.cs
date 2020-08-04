using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Items;

namespace SmartGrid
{
    [DataContract]
    public class TagGroup : HeaderableList<Tag>, DragProcessor.IDragElement
    {
        public TagGroup() : base()
        {
        }
    }
}
