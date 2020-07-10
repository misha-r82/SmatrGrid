using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;
using SmartGrid.Items;

namespace SmartGrid
{
    [DataContract]
    public class WorkSpace : HeaderableList<SmartFiled>
    {
        [DataMember] private SmartFiled _activeField;
        [DataMember] private static WorkSpace _instance;
        public UndoData Undo { get; private set; }

        public static WorkSpace Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                if (_instance.Undo == null) _instance.Undo = new UndoData();
                _instance.FireActiveFieldChanged();
            }
        }

        public WorkSpace()
        {
            Undo = new UndoData();
            Add(new SmartFiled("Раздел1"));
            Add(new SmartFiled("Раздел2"));
            Add(new SmartFiled("Раздел3"));
            ActiveField = this[0];
        }

        public SmartFiled ActiveField
        {
            get { return _activeField; }
            set
            {
                _activeField = value; 
                OnPropertyChanged();
            }
        }

        public new void Remove(SmartFiled field)
        {
            var pos = IndexOf(field);
            RemoveAt(pos);
            if (Count == 0)
            {
                ActiveField = new SmartFiled("Новый раздел");
                Add(ActiveField);
            } else if (!(pos < Count)) pos--;
            ActiveField = this[pos];
        }
        protected void FireActiveFieldChanged()
        {
            OnPropertyChanged(nameof(ActiveField));
        }
 }
}
