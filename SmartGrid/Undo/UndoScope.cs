using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;

namespace SmartGrid.Undo
{
    public abstract class UndoScope
    {
        public UndoScope(string name)
        {
            Name = name;
        }
        public virtual string Name { get; }
        public abstract void Undo();
        public abstract void Redo();
        public abstract bool HasChanges { get; }
    }
}
