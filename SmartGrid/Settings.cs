using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;

namespace SmartGrid
{
    public static class Sett
    {
        static Sett()
        {
            Settings = new Settings();
        }
        public static Settings Settings { get; }
    }
    public class Settings : INotifyPropertyChanged
    {
        private static bool _ctrlEnter;

        public bool CtrlEnter
        {
            get { return _ctrlEnter; }
            set
            {
                if (value == _ctrlEnter) return;
                _ctrlEnter = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
