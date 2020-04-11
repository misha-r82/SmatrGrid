using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SmartGrid
{
    [DataContract]
    public class Tag : INoteElement , IEnumerable<Node>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        [DataMember] private List<Node> Nodelist;
        [DataMember] public ViewStyle ViewStl { get; set; }
        [DataMember] private string _header;
        [DataMember] public FontStyle HeaderStyle { get; set; }
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        public Tag()
        {
            Nodelist = new List<Node>();
            ViewStl = new ViewStyle();
            HeaderStyle = new FontStyle();
        }
        public void Add(Node node, Node insertAfter = null, bool notify = true)
        {
            if (string.IsNullOrEmpty(node.Header)) return;

            if (insertAfter == null) Nodelist.Add(node);
            else
            {
                int pos = Nodelist.IndexOf(insertAfter);
                if (pos == -1) pos = Nodelist.Count - 1;
                if (Nodelist.Any(n => object.ReferenceEquals(n, node)))
                {
                    int posExist = Nodelist.IndexOf(insertAfter);
                    if (posExist < pos) pos--;
                    Nodelist.Remove(node);
                }
                    
                Nodelist.Insert(pos, node);
            }
            if (notify) OnCollectionChanged();
        }
        public void Add(IEnumerable<Node> nodes, Node insertAfter = null, bool cloneNodes = false)
        {
            foreach (Node node in nodes)
            {
                var added = cloneNodes ? node.GetClone() : node;
                Add(added, insertAfter, cloneNodes);
            }              
            OnCollectionChanged();
        }

        public void Remove(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
                Nodelist.Remove(node);
            OnCollectionChanged();
        }
        public void Remove(Node node)
        {
            Nodelist.Remove(node);
            OnCollectionChanged();
        }
        public Tag GetClone(TagWrap tWrap)
        {
            var clone = new Tag();
            clone.Header = Header;
            foreach (Node node in Nodelist)
                clone.Nodelist.Add(node);
            return clone;
        }
        public IEnumerator<Node> GetEnumerator()
        {
            return Nodelist.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodelist.GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


    }
}
