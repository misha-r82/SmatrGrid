﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class Node : ICloneableEx<Node>, DragProcessor.IDragElement, INotifyPropertyChanged
    {
        [DataMember] private byte[] _valBin;
        [DataMember] public HeaderClass Header { get; private set; }

        public Node(string header = "")
        {
            ViewStl = new ViewStyle();
            Header = new HeaderClass(header);
        }

        public Node() :this("") { }

        [DataMember] public ViewStyle ViewStl { get; set; }

        public byte[] ValBin
        {
            get { return _valBin; }
            set { _valBin = value; }
        }

        public object Clone()
        {
            return GetClone();
        }

        public void CloneRefs()
        {
            ViewStl = ViewStl.GetClone();
            Header = Header.GetClone();
        }

        DragProcessor.IDragElement ICloneableEx<DragProcessor.IDragElement>.GetClone()
        {
            return GetClone();
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
