using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartGrid.Annotations;
using static SmartGrid.DragProcessor;

namespace SmartGrid.Items
{
    [DataContract]
    [JsonObject]
    public class HeaderableList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged, DragProcessor.IContainer, IResetCollection<T> where T: class, IDragElement
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

        private bool CheckItem(T item)
        {
            if (string.IsNullOrEmpty(item.Header.Header)) return false;
            foreach (T listItem  in _list)
                if (listItem.Equals(item))
                    return false;
            return true;
        }
        public void Add(T addItem)
        {
            if (!CheckItem(addItem)) return;
            _list.Add(addItem);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addItem, _list.Count-1));
            OnPropertyChanged(nameof(Count));
        }
        public void Insert(int index, T insertItem)
        {
            if (!CheckItem(insertItem)) return;
            if (index < 0)
            {
                Add(insertItem);
                return;
            }
            _list.Insert(index, insertItem);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, insertItem, index));
            OnPropertyChanged(nameof(Count));
        }
        public void Add(T added, T insertBefore)
        {
            if (string.IsNullOrEmpty(added.Header.Header)) return;
            if (insertBefore == null)
            {
                Add(added);
                return;
            }
            if (insertBefore == added) return;
            int pos = _list.IndexOf(insertBefore);
            if (pos == -1) pos = _list.Count - 1;
            if (_list.Any(n => object.ReferenceEquals(n, added)))
            {
                int posExist = _list.IndexOf(added);
                if (posExist < pos) pos--;
                Remove(added);
            }
            Insert(pos, added);
        }

        public void Add(IEnumerable<T> items, T insertBefore)
        {
            foreach (T item in items.Reverse()) Add(item, insertBefore);
        }

        public virtual void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null)
        {
            Add(items.OfType<T>(), insertBefore as T);
        }

        public virtual void Remove(IEnumerable<IHasHeader> items)
        {
            Remove(items.OfType<T>());
        }

        public bool AcceptType(Type type)
        {
            return type == typeof(T);
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


        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }
        public void Reset(IEnumerable<T> newElements)
        {
            _list.Clear();
            _list.AddRange(newElements);
            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

        public object Clone()
        {
            var clone = (HeaderableList<T>) MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }

        public void CloneRefs()
        {
             _list = new List<T>(_list.Select(item=>item.Clone() as T));
            _header = _header.GetClone();           
        }

        public IDragElement GetClone()
        {
            return (HeaderableList<T>) Clone();
        }

    }

}
