using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Undo;

namespace SmartGrid
{
    public class UndoData
    {
        public UndoData()
        {
            UndoStack = new Stack<UndoScope>();
            RendoStack = new Stack<UndoScope>();
        }
        public Stack<UndoScope> UndoStack { get; }
        public Stack<UndoScope> RendoStack { get; }

        public void Undo()
        {
            var undo = UndoStack.Pop();
            RendoStack.Push(undo);
            undo.Undo();
        }
        public void Rendo()
        {
            var rendo = RendoStack.Pop();
            UndoStack.Push(rendo);
            rendo.Rendo();
        }

    }
}
