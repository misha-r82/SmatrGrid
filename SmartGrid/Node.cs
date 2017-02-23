using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class Node : IHeader
    {
        private string _val;
        private string _header;
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
    }
}
