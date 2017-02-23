using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmartGrid
{
    class SmartFiled
    {
        public Tag[,] Cells;

        public SmartFiled()
        {
            Cells = new Tag[3,3];
            for (int i = 0; i<Cells.GetLength(0); i++)
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j] = new Tag() {Header = i.ToString() + j};
                }
                    
        }

        
    }
}
