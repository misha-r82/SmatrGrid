using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Undo
{
    public class NodeListScope : UndoScope
    {
        private Tag _tag;
        private Node[] _oldList;
        public NodeListScope(Tag tag)
        {
            _tag = tag;
            _oldList = tag.ToArray();
        }

        private void Swap()
        {
            var tmp = _tag.ToArray();
            _tag.Clear(false);
            _tag.Add(_oldList);
            _oldList = tmp;
        }
        public override void Undo()
        {
            Swap();
        }

        public override void Rendo()
        {
            Swap();
        }
        public override bool HasChanges
        {
            get
            {
                if (_oldList.Length != _tag.Count) return true;
                int pos = 0;
                foreach (Node node in _tag)
                {
                    if (pos < _oldList.Length) pos++;
                    else return true;
                    if (!node.Equals(_oldList[pos])) return true;
                }
                return false;
            }

        }
    }
}
