using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Items
{
    [DataContract]
    public class HeaderableList<T> : IList<T>, INotifyCollectionChanged, IHasHeader where T:IHasHeader
    {
        [DataMember] private List<T> _list;
        [DataMember] private HeaderClass _header;
        public T this[int index] { get => _list[index]; set => _list[index] = value; }

        public HeaderClass Header { get; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            CollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        public void Add(T item, Node insertAfter = null)
        {
            
        }

        public void Add(T item)
        {
            
        }

        public void Clear()
        {
            _list.Clear();
            on
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public bool Remove(T item)
        {

            int index = _list.IndexOf(item);
            if (_list.Remove(item))
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
                return true;
            }

            return false;
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
    }
}
