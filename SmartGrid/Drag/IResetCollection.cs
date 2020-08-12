using System;
using System.Collections;
using System.Collections.Generic;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IResetCollection<T> : IEnumerable<T>
        {
            void Reset(IEnumerable<T> newElements);
        }
    }
}