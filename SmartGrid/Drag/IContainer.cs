using System;
using System.Collections.Generic;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IContainer : IDragElement
        {
            void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null);
            void Remove(IEnumerable<IHasHeader> items);
            bool AcceptType(Type type);
        }
    }
}