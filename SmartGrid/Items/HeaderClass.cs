using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SmartGrid.Annotations;
using SmartGrid.HeaderIcons;
using Test;

namespace SmartGrid
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Node))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(Tag))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TagGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartFiled))]
    public interface IHasHeader
    {
        HeaderClass Header { get; }
    }
    [DataContract]
    public class HeaderClass : INotifyPropertyChanged, ICloneableEx<HeaderClass>
    {
        [DataMember]private FontStyle style;
        [DataMember] private string _header;
        [DataMember] private IconSet _icons;

        public HeaderClass(string header = "")
        {
            _header = header;
            Style = new FontStyle();
            _icons = new IconSet();
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
        public FontStyle Style
        {
            get => style;
            set => style = value;
        }

        public IconSet Icons => _icons;

        public void CloneRefs()
        {
            Style = style.GetClone();
            _icons = new IconSet(_icons);
            
        }
        public object Clone()
        {
            return GetClone();
        }
        public HeaderClass GetClone()
        {
            var clone = (HeaderClass)MemberwiseClone();
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