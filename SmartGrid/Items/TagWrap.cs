using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SmartGrid.Annotations;
using SmartGrid.Undo;

namespace SmartGrid
{
    [DataContract]
    public class TagWrap: INotifyPropertyChanged, IHasHeader, DragProcessor.IContainer<Tag>
    {
        [DataMember] private Tag _tag;
        public HeaderClass Header => _tag.Header;
        public TagUndoScope TagUndoScope { get; private set; }
        public TagWrap(string header = "")
        {
            _tag = new Tag(header);
        }
        public TagWrap(Tag tag)
        {
            _tag = tag;
        }
        public Tag Tag
        {
            get { return _tag; }
            set
            {
                if (Equals(value, _tag)) return;
                TagUndoScope = new TagUndoScope(this);
                _tag = value;
                OnPropertyChanged();
            }
        }
        public TagWrap GetClone()
        {
            var clone = (TagWrap)MemberwiseClone();
            clone._tag = _tag.GetClone();
            return clone;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Add(Tag[] items, Tag insertBefore)
        {
            Tag = items.First();
        }

        public void Remove(Tag[] item)
        {
            Tag = new Tag();            
        }
    }
}