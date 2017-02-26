using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class Tag : IHeader , IEnumerable<Node>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private const int HEAD_INDEX = Int32.MinValue;
        private SortedList<int, Node> Nodelist;
        private string _header;
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        public Tag()
        {
            Nodelist = new SortedList<int, Node>();
            var head = new HeadNode(this) {Header = "headHeder", Val = "headVal"};
            Nodelist.Add(HEAD_INDEX, head);
            var node = new Node() { Header = "Header", Val = "Val" };
            Nodelist.Add(0, node);
        }
        public void Add(Node node, bool notify = true)
        {
            var topKey = Nodelist.Keys[Nodelist.Count - 1];
            Nodelist.Add(topKey+1, node);
            if (notify) OnCollectionChanged();
        }
        public void Add(IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
                Add(node, false);
            OnCollectionChanged();
        }
        public void Remove(Node node)
        {
            
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return Nodelist.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Nodelist.Values.GetEnumerator();
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
