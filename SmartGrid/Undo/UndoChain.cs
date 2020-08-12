using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SmartGrid.Undo
{
    public class UndoChain : UndoScope
    {
        public UndoChain(string name) : base(name)
        {
            _chain = new List<UndoScope>();
        }
        private List<UndoScope> _chain;
        public void Add(UndoScope scope)
        {
            _chain.Add(scope);
        }
        public override void Undo()
        {
            foreach (UndoScope scope in _chain)
                if (scope.HasChanges)
                    scope.Undo();
        }
        public override void Redo()
        {
            foreach (UndoScope scope in _chain)
                if (scope.HasChanges)
                    scope.Redo();
        }
        public override bool HasChanges
        {
            get
            {
                foreach (var scope in _chain)
                    if (scope.HasChanges)
                        return true;
                return false;
            }
        }
    }
}