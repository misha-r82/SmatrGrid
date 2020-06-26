using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using SmartGrid.Items;
using Test;

namespace SmartGrid
{
    [DataContract]
    public class Tag : HeaderableList<Node>, INotifyPropertyChanged, ICloneableEx<Tag>
    {
        [DataMember] public ViewStyle ViewStl { get; set; }

        public Tag(string header = "") : base(header)
        {
            ViewStl = new ViewStyle();
        }
        public bool IsEmptyTag
        {
            get => !this.Any() && string.IsNullOrEmpty(Header.Header);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object Clone()
        {
            return GetClone();
        }
        public void CloneRefs()
        {
            var tmp = this.ToList();
            Clear();
            foreach (Node node in tmp)
                Add(node);
            Header.CloneRefs();
        }
        public Tag GetClone()
        {
            var clone = (Tag)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }
    }
}
