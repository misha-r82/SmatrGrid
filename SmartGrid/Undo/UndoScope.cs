using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;

namespace SmartGrid.Undo
{
    public abstract class UndoScope : INotifyPropertyChanged
    {
        public virtual string Name { get; set; }
        public bool IsApplyed { get; }
        public abstract void Undo();
        public abstract void Rendo();
        public abstract bool HasChanges { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
