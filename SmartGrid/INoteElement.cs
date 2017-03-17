using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    interface INoteElement
    {
        string Header { get; set; }
        ViewStyle ViewStl { get; set; }
    }
}
