using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartGrid.Annotations;
using SmartGrid.Grid;

namespace SmartGrid
{
    [DataContract]
    public class GridWidth : INotifyPropertyChanged
    {
        [DataMember] private Double _w1, _w2, _w3, _h1, _h2, _h3;
        [DataMember] public double WidthAll { get; set; }
        public WidthManager Manager { get; }

        public GridLength GW1
        {
            get { return new GridLength(_w1, GridUnitType.Star); }
            set
            {
                _w1 = value.Value;
                Debug.WriteLine("w1={0}", _w1);
            }
        }
        public GridLength GW2
        {
            get { return new GridLength(_w2, GridUnitType.Star); }
            set
            {
                _w2 = value.Value;
                Debug.WriteLine("w2={0}", _w2);
            }
        }
        public GridLength GW3
        {
            get { return new GridLength(_w3, GridUnitType.Star); }
            set
            {
                _w3 = value.Value;
               Debug.WriteLine("w3={0}", _w3);
            }
        }
        public GridLength GH1
        {
            get { return new GridLength(_h1, GridUnitType.Star); }
            set
            {
                _h1 = value.Value;
                Debug.WriteLine("h1={0}", _h1);
            }
        }
        public GridLength GH2
        {
            get { return new GridLength(_h2, GridUnitType.Star); }
            set
            {
                _h2 = value.Value;
                Debug.WriteLine("h2={0}", _h2);
            }
        }
        public GridLength GH3
        {
            get { return new GridLength(_h3, GridUnitType.Star); }
            set
            {
                _h3 = value.Value;
               // Debug.WriteLine("h3={0}", _h3);
            }
        }

        public GridWidth()
        {
            Manager = new WidthManager(this);
            Manager.ShowAll();
        }
        public double W1
        {
            get { return _w1; }
            set
            {
                if (value.Equals(_w1)) return;
                _w1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GW1));
            }
        }
        public double W2
        {
            get { return _w2; }
            set
            {
                if (value.Equals(_w2)) return;
                _w2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GW2));
            }
        }
        public double W3
        {
            get { return _w3; }
            set
            {
                if (value.Equals(_w3)) return;
                _w3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GW3));
            }
        }
        public double H1
        {
            get { return _h1; }
            set
            {
                if (value.Equals(_h1)) return;
                _h1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GH1));
            }
        }

        public double H2
        {
            get { return _h2; }
            set
            {
                if (value.Equals(_h2)) return;
                _h2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GH2));
            }
        }
        public double H3
        {
            get { return _h3; }
            set
            {
                if (value.Equals(_h3)) return;
                _h3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GH3));
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
