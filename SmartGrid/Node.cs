using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace SmartGrid
{
    class Node : INoteElement, ICloneableEx<Node>
    {
        private string _val;
        private string _header;

        public Node(Tag tag)
        {
            Tag = tag;
            ViewStl = new ViewStyle();
        }
        public Tag Tag { get; set; }
        public string Val
        {
            get { return _val; }
            set { _val = value; }
        }
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
        public ViewStyle ViewStl { get; set; }
        public object Clone()
        {
            var clone = (Node)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }

        public void CloneRefs() { }

        public Node GetClone()
        {
            var clone = (Node)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }
    }
}
