using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SmartGrid.HeaderIcons
{
    [DataContract(IsReference = true)]
    public class IconCollection : IEnumerable<IconElement>, INotifyCollectionChanged
    {
        [DataMember]private ObservableCollection<IconElement> Items { get; set; }

        private IconCollection() 
        {
            Items = new ObservableCollection<IconElement>();
            Items.CollectionChanged += (sender, args) =>
            {
                CollectionChanged?.Invoke(sender, args);
            };
        }

        public IconCollection(string name, IconsGroup group) : this()
        {
            CreatElement(name);
            Group = group;
        }

        public IconElement FirstIcon => Items[0];
        [DataMember] public IconsGroup Group { get; private set; }

        public IconElement CreatElement(string name, Stream stream = null)
        {
            var element = new IconElement(name, this);
            element.FromStream(stream);
            Items.Add(element);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return element;
        }

        public IconElement NextIcon(IconElement icon)
        {
            int pos = Items.IndexOf(icon);
            if (pos < 0) throw new ArgumentException("icon is not contains in Collection");
            if (pos == Items.Count - 1) return Items[0];
            return Items[pos + 1];
        }

        public void Remove(IconElement icon)
        {
            Items.Remove(icon);
        }

        public IEnumerator<IconElement> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

}