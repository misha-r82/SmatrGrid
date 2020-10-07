using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SmartGrid.Annotations;
using SmartGrid.HeaderIcons;
using Test;

namespace SmartGrid
{
    public interface IHasHeader
    {
        HeaderClass Header { get; }
    }
    [DataContract]
    public class HeaderClass : INotifyPropertyChanged, ICloneableEx<HeaderClass>
    {
        [DataMember]private FontStyle style;
        [DataMember] private string _header;
        [DataMember] private IconCollection _icons;

        public HeaderClass(string header = "")
        {
            _header = header;
            Style = new FontStyle();
            _icons = new IconCollection();
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

        public IconCollection Icons => _icons;

        public void CloneRefs()
        {
            Style = style.GetClone();
            _icons = new IconCollection(_icons);
            
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