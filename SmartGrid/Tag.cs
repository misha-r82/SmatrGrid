using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class Tag : IHeader
    {
        private const int HEAD_INDEX = Int32.MaxValue;
        private SortedList<int, Node> Nodelist;
        private string _header;

        public Tag()
        {
            Nodelist = new SortedList<int, Node>();
            var head = new Node();
            Nodelist.Add(HEAD_INDEX, head);
        }
        public void Add(Node node)
        {
            
        }

        public void Remove(Node node)
        {
            
        }

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
    }
}
