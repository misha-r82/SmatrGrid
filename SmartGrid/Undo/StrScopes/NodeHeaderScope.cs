using System;

namespace SmartGrid.Undo
{
    public class NodeHeaderScope : StrEditScope<Node>
    {
        public NodeHeaderScope(Node obj) : 
            base("Редактирование заголовка {0}", n=>n.Header, (node, str)=>node.Header = str, obj)
        {
        }
    }
}