using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lib;
using SmartGrid.Annotations;
using SmartGrid.Undo;

namespace SmartGrid
{
    public class UndoData : INotifyPropertyChanged
    {
        public UndoData()
        {
            UndoStack = new ObservableStack<UndoScope>();
            RedoStack = new ObservableStack<UndoScope>();
        }
        public ObservableStack<UndoScope> UndoStack { get; }
        public ObservableStack<UndoScope> RedoStack { get; }

        public void AddScope(UndoScope scope)
        {
            if (scope == null) return;
            if (!scope.HasChanges) return;
            RedoStack.Clear();
            UndoStack.Push(scope);
            if (UndoStack.Count == 1) OnPropertyChanged(nameof(CanUndo));
        }

        public void UndoToScope(UndoScope scopeTo)
        {
            UndoScope scope;
            do
            {
                scope = UndoStack.Peek();
                Undo();
            } while (!scopeTo.Equals(scope));
        }
        public void RedoToScope(UndoScope scopeTo)
        {
            UndoScope scope;
            do
            {
                scope = RedoStack.Peek();
                Redo();
            } while (!scopeTo.Equals(scope));
        }
        public bool CanUndo => UndoStack.Count > 0;
        public bool CanRedo => RedoStack.Count > 0;
        public void Undo()
        {
            var undo = UndoStack.Pop();
            RedoStack.Push(undo);
            undo.Undo();
            if (UndoStack.Count > 0) OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
        }
        public void Redo()
        {
            var redo = RedoStack.Pop();
            UndoStack.Push(redo);
            redo.Redo();
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
