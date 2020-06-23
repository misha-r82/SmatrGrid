using System;

namespace SmartGrid.Undo
{
    public class HeaderUndoScope<T> : StrEditScope<T> where T : IHasHeader
    {
        public HeaderUndoScope(T obj, string scopeNamePatt) : base(scopeNamePatt,
            o=>obj.Header.Header, (
                 h, s) =>h.Header.Header = s,
                 obj) { }
    }
}