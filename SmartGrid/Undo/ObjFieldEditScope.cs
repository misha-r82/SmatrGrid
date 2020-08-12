using System;

namespace SmartGrid.Undo
{
    public abstract class ObjFieldEditScope<Tobj, Tfield> : UndoScope
    {
        protected Tfield _oldValue;
        protected Func<Tobj, Tfield> _getter;
        private Action<Tobj, Tfield> _setter;
        protected Tobj _obj;
        public ObjFieldEditScope(string name, Func<Tobj, Tfield> getter, Action<Tobj, Tfield> setter, Tobj obj) : base(name)
        {
            _oldValue = getter(obj);
            _getter = getter;
            _setter = setter;
            _obj = obj;
        }
        public override bool HasChanges =>  !ReferenceEquals(_oldValue, _getter(_obj));

        public override void Undo()
        {
            Swap();
        }

        public override void Redo()
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