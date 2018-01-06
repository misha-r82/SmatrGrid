using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class TagGroup : INoteElement, INotifyPropertyChanged
    {
        public TagGroup()
        {
            TagList = new List<Tag>();
        }
        private string _header;

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public void Add(Tag tag)
        {
            TagList.Add(tag);
            OnPropertyChanged(nameof(TagList));
        }

        public void Add(IEnumerable<Tag> tags)
        {
            TagList.AddRange(tags);
            OnPropertyChanged(nameof(TagList));
        }
        public void Remove(Tag tag)
        {
            OnPropertyChanged(nameof(TagList));
        }
        public List<Tag> TagList { get; }
        public ViewStyle ViewStl { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
