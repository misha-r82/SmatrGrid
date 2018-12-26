using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    [DataContract]
    public class TagGroup : /*INoteElement,*/ INotifyPropertyChanged
    {
        public TagGroup()
        {
            TagList = new ObservableCollection<TagWrap>();
        }
        public void Add(IEnumerable<TagWrap> tags)
        {
            foreach (var tag in tags)
                TagList.Add(tag);
        }
        public void Remove(TagWrap tag)
        {
            TagList.Remove(tag);
            OnPropertyChanged(nameof(TagList));
        }
        [DataMember] public ObservableCollection<TagWrap> TagList { get; private set; }
        //public ViewStyle ViewStl { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
