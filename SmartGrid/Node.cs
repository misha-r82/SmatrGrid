﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;
using Test;

namespace SmartGrid
{
    [DataContract]
    public class Node : INoteElement, ICloneableEx<Node>, INotifyPropertyChanged
    {
        [DataMember] private string _val;
        [DataMember] private string _header;

        public Node()
        {
            ViewStl = new ViewStyle();
        }
        public string Val
        {
            get { return _val; }
            set
            {
                if (_val == value) return;
                _val = value;
                OnPropertyChanged();
            }
        }
        public string Header
        {
            get { return _header; }
            set
            {
                if (_header == value) return;
                _header = value;
                OnPropertyChanged();
            }
        }
        [DataMember] public ViewStyle ViewStl { get; set; }
        public object Clone()
        {
            var clone = (Node)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }

        public void CloneRefs()
        {
            ViewStl = ViewStl.GetClone();
        }

        public Node GetClone()
        {
            var clone = (Node)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
