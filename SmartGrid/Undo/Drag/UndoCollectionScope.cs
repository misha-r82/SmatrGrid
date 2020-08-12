using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static SmartGrid.DragProcessor;

namespace SmartGrid.Undo
{
    public static class UndoCollectionScopeFactory
    {
        public static UndoScope CreateScope(IContainer container)
        {
            if (container is IResetCollection<Node>) return new UndoCollectionScope<Node>((IResetCollection<Node>)container);
            if (container is IResetCollection<Tag>) return new UndoCollectionScope<Tag>((IResetCollection<Tag>)container);
            if (container is IResetCollection<SmartFiled>) return new UndoCollectionScope<SmartFiled>((IResetCollection<SmartFiled>)container);
            throw new ArgumentException("Недопустимый тип IContayner!");

        }
    }
    public class UndoCollectionScope<T> : UndoScope where T:IHasHeader
    {
        private IResetCollection<T> _collection;
        private T[] _origElements;
        public UndoCollectionScope(IResetCollection<T> collection):base("")
        {
            _origElements = collection.ToArray();
            _collection = collection;
        }

        private void Swap()
        {
            var newElements = _collection.ToArray();
            _collection.Reset(_origElements);
            _origElements = newElements;
        }
        public override void Undo()
        {
            Swap();
        }

        public override void Redo()
        {
            Swap();
        }

        public override bool HasChanges
        {
            get
            {
                if (_collection.Count() != _origElements.Length) return true;
                var pos = 0;
                foreach (var item in _collection)
                    if (!Equals(_origElements[pos++], item))
                        return true;
                return false;
            }
        }
    }
}
