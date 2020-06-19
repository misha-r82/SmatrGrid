using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SmartGrid.Undo
{
    public abstract class StrEditScope<T> : UndoScope
    {
        private const int MAX_LEN = 24;
        private string _oldValue;
        protected string scopeNamePatt;
        private Func<T, string> _getter;
        private Action<T, string> _setter;
        private T _obj;
        public StrEditScope(string scopeNamePatt, Func<T, string> getter, Action<T, string> setter, T obj)
        {
            _oldValue = getter(obj);
            _getter = getter;
            _setter = setter;
            _obj = obj;
            this.scopeNamePatt = scopeNamePatt;
        }
        public override string Name
        {
            get
            {
                var str = _oldValue.Length > MAX_LEN ? 
                    _oldValue.Substring(0, MAX_LEN) + "..." : _oldValue;
                return string.Format(scopeNamePatt, str);
            }
        }

        public override bool HasChanges => _oldValue != _getter(_obj);

        public override void Undo()
        {
            Swap();
        }

        public override void Rendo()
        {
            Swap();
        }

        private void Swap()
        {
            var tmp = _oldValue;
            _oldValue = _getter(_obj);
            _setter(_obj, tmp);

        }
    }
}
