using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class Node : INoteElement
    {
        private string _val;
        private string _header;

        public Node()
        {
            ViewStl = new ViewStyle();
        }
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
    }
}
