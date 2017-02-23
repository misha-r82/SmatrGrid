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
        private Tag _tag;
        public HeadNode(Tag tag) : base()
        {
            _tag = tag;
        }

        public void AddToTag()
        {
            var newNode = new Node() {Header = Header};
            _tag.Add(newNode);
        }
    }
}
