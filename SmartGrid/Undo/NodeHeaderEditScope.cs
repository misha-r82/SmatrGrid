using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Undo
{
    class NodeHeaderEditScope : UndoScope
    {
        private Node _node;
        private string _oldHeader;
        public NodeHeaderEditScope(Node node)
        {
            _node = node;
            _oldHeader = node.Header;
            Name = $"Редактирование элемета {node.Header}";
        }
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
            var tmp = _node.Header;
            _node.Header = _oldHeader;
            _oldHeader = tmp;
        }
    }
}
