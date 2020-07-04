using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;

namespace SmartGrid.Items
{
    [DataContract]
    public class HeaderableList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged, IHasHeader where T:IHasHeader
    {
        [DataMember] private List<T> _list;
        [DataMember] private HeaderClass _header;

        public HeaderableList(string name = "")
        {
            _header = new HeaderClass(name);
            _list = new List<T>();
        }
        public T this[int index] { get => _list[index]; set => _list[index] = value; }

        public HeaderClass Header
        {
            get => _header;
            set
            {
                if (_header == value) return;
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged == null) return;
            CollectionChanged(this, args);
        }

        public void Add(T addItem)
        {
            foreach (T item  in _list)
                if (item.Equals(addItem))
                    return;
            _list.Add(addItem);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addItem, _list.Count-1));
            OnPropertyChanged(nameof(Count));
        }
        public void Add(T added, T insertAfter)
        {
            if (string.IsNullOrEmpty(added.Header.Header)) return;
            if (insertAfter == null)
            {
                Add(added);
                return;
            }
            int pos = _list.IndexOf(insertAfter);
            if (pos == -1) pos = _list.Count - 1;
            if (_list.Any(n => object.ReferenceEquals(n, added)))
            {
                int posExist = _list.IndexOf(insertAfter);
                if (posExist < pos) pos--;
                Remove(added);
            }
            Insert(pos, added);
        }
        public void Add(IEnumerable<T> items, T insertAfter = default(T))
        {
            foreach (T item in items) Add(item, insertAfter);
        }
        public void Insert(int index, T insertItem)
        {
            foreach (T item in _list)
                if (item.Equals(insertItem))
                    return;
            _list.Insert(index, insertItem);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, insertItem, index));
            OnPropertyChanged(nameof(Count));
        }
        public bool Remove(T item)
        {

            int index = _list.IndexOf(item);
            if (_list.Remove(item))
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                OnPropertyChanged(nameof(Count));
                return true;
            }
            return false;
        }

        public void Remove(IEnumerable<T> items)
        {
            foreach (T item in items) Remove(item);
        }
        public void Clear()
        {
            _list.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
