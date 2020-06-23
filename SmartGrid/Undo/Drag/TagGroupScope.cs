using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Undo.Drag
{
    public class TagGroupScope : UndoScope
    {

        public TagGroupScope(TagGroup grp)
        {

        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override void Rendo()
        {
            throw new NotImplementedException();
        }

        public override bool HasChanges { get; }
    }
}
