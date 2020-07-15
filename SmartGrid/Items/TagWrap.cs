using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SmartGrid.Annotations;
using SmartGrid.Undo;

namespace SmartGrid
{
    [DataContract]
    public class TagWrap: INotifyPropertyChanged, IHasHeader, DragProcessor.IContainer
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
        public void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore)
        {
            var tag = items.First() as TagWrap;
            if (tag != null)
                Tag = tag.Tag;
            else
            {
                var nodes = items.Cast<Node>();
                Tag.Add(nodes, insertBefore);
            }
        }

        public void Remove(IEnumerable<IHasHeader> items)
        {
            if (items.First() is TagWrap)
                Tag = new Tag();
            else Tag.Remove(items.Cast<Node>());
        }
    }
}