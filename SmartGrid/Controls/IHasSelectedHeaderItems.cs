using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Controls
{
    public interface IHasSelectedHeaderItems
    {
        IEnumerable<IHasHeader> SelectedItems { get; }
    }
}
