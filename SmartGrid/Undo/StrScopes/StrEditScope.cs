using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace SmartGrid.Undo
{
    public abstract class StrEditScope<T> : ObjFieldEditScope<T, string>
    {
        private const int MAX_LEN = 24;
        private string scopeNamePatt;

        public StrEditScope(string scopeNamePatt, Func<T, string> getter, Action<T, string> setter, T obj) : base("", getter, setter, obj)
        {
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
    }
}
