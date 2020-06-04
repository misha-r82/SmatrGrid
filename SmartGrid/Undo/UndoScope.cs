using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Undo
{
    public abstract class UndoScope
    {
        public string Name { get; set; }
        public bool IsApplyed { get; set; }
        public abstract void Undo();
        public abstract void Rendo();
    }
}
