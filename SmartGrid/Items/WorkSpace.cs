using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SmartGrid.Annotations;

namespace SmartGrid
{
    [DataContract]
    public class WorkSpace : INotifyPropertyChanged
    {
        [DataMember] private SmartFiled _activeField;
        [DataMember] private static WorkSpace _instance;

        public static WorkSpace Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.FireActiveFieldChanged();
            }
        }

        public WorkSpace()
        {
            FieldList = new ObservableCollection<SmartFiled>();   
            FieldList.Add(new SmartFiled() { Header = "Раздел1" });
            FieldList.Add(new SmartFiled() { Header = "Раздел2" });
            FieldList.Add(new SmartFiled() { Header = "Раздел3" });
            ActiveField = FieldList[0];
        }
        [DataMember] public ObservableCollection<SmartFiled> FieldList { get; set; }

        public SmartFiled ActiveField
        {
            get { return _activeField; }
            set
            {
                _activeField = value; 
                OnPropertyChanged();
            }
        }

        public void Remove(SmartFiled field)
        {
            var pos = FieldList.IndexOf(field);
            FieldList.RemoveAt(pos);
            if (FieldList.Count == 0)
            {
                ActiveField = new SmartFiled("Новый раздел");
                FieldList.Add(ActiveField);
            } else if (!(pos < FieldList.Count)) pos--;
            ActiveField = FieldList[pos];
        }
        protected void FireActiveFieldChanged()
        {
            OnPropertyChanged(nameof(ActiveField));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
