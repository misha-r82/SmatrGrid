using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SmartGrid.Annotations;
using Test;
using static SmartGrid.DragProcessor;

namespace SmartGrid
{
    [DataContract]
    public class SmartFiled : INotifyPropertyChanged, ICloneableEx<SmartFiled>, DragProcessor.IContainer, IResetCollection<Tag>
    {
        [DataMember]
        public Tag WorkTag
        {
            get => Cells[9];
            set
            {
                if (Cells[9] == value) return;
                Cells[0] = value;
                OnPropertyChanged(nameof(WorkTag));
            }
        }

        [DataMember] public TagGroup TagGrp { get; private set; }
        [DataMember] public GridWidth GridWidth { get; private set; }
        private bool _isEditMode;
        [DataMember]public HeaderClass Header { get; private set; }
        public ObservableCollection<Tag> Cells { get; private set; }
        private Tag _workTag;

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode == value) return;
                _isEditMode = value;
                OnPropertyChanged();
            }
        }
        public SmartFiled(string header = "")
        {
            Header = new HeaderClass(header);
            TagGrp = new TagGroup();
            GridWidth = new GridWidth();
            TagGrp.Add(new Tag("Корзина"));
            TagGrp.Add(new Tag("Важное"));
            Cells = new ObservableCollection<Tag>();
            for (int i = 0; i < 9; i++) Cells.Add(new Tag((i + 1).ToString()));
            Cells.Add(new Tag("текущий"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null)
        {
            var pos = Cells.IndexOf(insertBefore as Tag);
            if (pos < 0) return;
            Cells[pos] = (Tag)items.First();
        }

        public void Remove(IEnumerable<IHasHeader> items)
        {
            var pos = Cells.IndexOf(items.First() as Tag);
            if (pos <0) return;
            Cells[pos] = new Tag();
         }

        public bool AcceptType(Type type)
        {
            return type == typeof(Tag);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void CloneRefs()
        {

            Cells = new ObservableCollection<Tag>(Cells.Select(cell=>cell.GetClone()));
            Header = Header.GetClone();
        }

        IDragElement ICloneableEx<IDragElement>.GetClone()
        {
            return GetClone();
        }

        public SmartFiled GetClone()
        {
            var clone = (SmartFiled)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }

        public void Reset(IEnumerable<Tag> newElements)
        {
            int pos = 0;
            foreach (var item in newElements) Cells[pos++] = item;
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Cells.GetEnumerator();
        }
    }
}
