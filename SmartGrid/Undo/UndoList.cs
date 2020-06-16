using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;
using SmartGrid.Undo;

namespace SmartGrid
{
    public class UndoData : INotifyPropertyChanged
    {
        public UndoData()
        {
            UndoStack = new Stack<UndoScope>();
            RedoStack = new Stack<UndoScope>();
        }
        public Stack<UndoScope> UndoStack { get; }
        public Stack<UndoScope> RedoStack { get; }

        public void AddScope(UndoScope scope)
        {
            RedoStack.Clear();
            UndoStack.Push(scope);
            if (UndoStack.Count == 1) OnPropertyChanged(nameof(CanUndo));
        }
        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;
        public void Undo()
        {
            var undo = UndoStack.Pop();
            RedoStack.Push(undo);
            undo.Undo();
            if (UndoStack.Count > 0) OnPropertyChanged(nameof(CanUndo));
        }
        public void Redo()
        {
            var rendo = RedoStack.Pop();
            UndoStack.Push(rendo);
            rendo.Rendo();
            if (RedoStack.Count > 0) OnPropertyChanged(nameof(CanRedo));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
