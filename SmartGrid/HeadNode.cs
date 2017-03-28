using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class HeadNode : Node
    {
        public HeadNode(Tag tag) : base(tag) { }

        public void AddToTag()
        {
            var newNode = new Node(Tag) {Header = Header};
            Tag.Add(newNode);
        }
    }
}
