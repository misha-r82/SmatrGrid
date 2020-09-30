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
using SmartGrid.HeaderIcons;
using SmartGrid.Items;

namespace SmartGrid
{
    [DataContract]
    public class WorkSpace : HeaderableList<SmartFiled>
    {
        [DataMember] private SmartFiled _activeField;
        [DataMember] private HeaderIcons.HeaderIcon _coreHeaderIcon;
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

        public HeaderIcon CoreHeaderIcon => _coreHeaderIcon;
        public WorkSpace()
        {
            Undo = new UndoData();
            Add(new SmartFiled("Раздел1"));
            Add(new SmartFiled("Раздел2"));
            Add(new SmartFiled("Раздел3"));
            ActiveField = this[0];
            _coreHeaderIcon = HeaderIcon.CreateBaseItem();
            _coreHeaderIcon.Name = "Icons";
            _coreHeaderIcon.IconCollection.Add(_coreHeaderIcon.CreateChield("1"));
            _coreHeaderIcon.IconCollection.Add(_coreHeaderIcon.CreateChield("2"));
            _coreHeaderIcon.IconCollection.Add(_coreHeaderIcon.CreateChield("3"));
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

        public override void Remove(IEnumerable<IHasHeader> items)
        {
            var field = (SmartFiled)items.First();
            base.Remove(field);
            if (Count == 0)
            {
                ActiveField = new SmartFiled("Новый раздел");
                Add(ActiveField);
            }
        }

        public override void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null)
        {
            base.Add(items, insertBefore);
            ActiveField = items.First() as SmartFiled;
        }

        protected void FireActiveFieldChanged()
        {
            OnPropertyChanged(nameof(ActiveField));
        }
 }
}
