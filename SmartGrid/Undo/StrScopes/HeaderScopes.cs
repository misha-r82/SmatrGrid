using System;

namespace SmartGrid.Undo
{
    public class NodeHeaderScope : StrEditScope<Node>
    {
        public NodeHeaderScope(Node obj) : 
            base("Редактирование заголовка тезиса {0}", n=>n.Header, (node, str)=>node.Header = str, obj)
        {
        }
    }
    public class TagHeaderScope : StrEditScope<Tag>
    {
    public TagHeaderScope(Tag obj) :
        base("Редактирование заголовка набора {0}", t => t.Header, (t, str) => t.Header = str, obj)
    {
    }
    }
    public class FieldHeaderScope : StrEditScope<SmartFiled>
    {
        public FieldHeaderScope(SmartFiled obj) :
            base("Редактирование раздела {0}", t => t.Header, (t, str) => t.Header = str, obj)
        {
        }
    }
}