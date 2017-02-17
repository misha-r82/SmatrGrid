using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    class TagGroup : IHeader
    {
        private string _header;

        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }
    }
}
