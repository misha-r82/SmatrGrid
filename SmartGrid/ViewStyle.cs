using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid
{
    public class ViewStyle : INotifyPropertyChanged
    {
        private bool _detailsVisile;

        public bool DetailsVisile
        {
            get { return _detailsVisile; }
            set
            {
                if (_detailsVisile == value) return;
                _detailsVisile = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
