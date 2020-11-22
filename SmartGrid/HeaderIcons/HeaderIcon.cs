using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using SmartGrid.Annotations;

namespace SmartGrid.HeaderIcons
{
    [DataContract]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(System.Array))]  //here !!!!! for System.Byte[*]
    public class IconData
    {
        [DataMember]
        private byte[] binData;
        public IconData()
        { }

        public IconData(string name)
        {
            Name = name;
        }

        [DataMember]
        public string Name { get; set; }

        public BitmapImage IconBitMap { get; private set; }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (binData == null) return;
            var stream = new MemoryStream(binData);
            FromStream(stream);
        }

        public void FromStream(Stream stream)
        {
            if (stream == null) return;
            using (BinaryReader br = new BinaryReader(stream))
            {
                binData = br.ReadBytes((int) stream.Length);
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(binData);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                IconBitMap = bitmap;
            }
            catch (Exception e)
            {
            }

        }
    }

    [DataContract(IsReference = true)]
    public class HeaderIcon : INotifyPropertyChanged
    {
        [DataMember]
        private readonly IconData _icon;
        public IconData Icon => _icon;
        public HeaderIcon CurIcon => IconCollection[_curPos];
        [DataMember]
        public ObservableCollection<HeaderIcon> IconCollection { get; private set; }
        [DataMember] public HeaderIcon Parent { get; private set; }
        [DataMember] private int _curPos;
        private HeaderIcon()
        {
            _icon = new IconData("Base Icon");
            IconCollection = new ObservableCollection<HeaderIcon>(new List<HeaderIcon>());
            IconCollection.Add(this);
        }
        public static HeaderIcon CreateBaseItem() { return new HeaderIcon();}
        public HeaderIcon Create(HeaderIcon parent, string name, Stream stram)
        {
            var newItem = new HeaderIcon();
            newItem.Icon.Name = name;
            newItem.Icon.FromStream(stram);
            newItem.Parent = parent;
            IconCollection.Add(newItem);
            return newItem;
        }
        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if(IconCollection.Count == 0) IconCollection.Add(this);
        }
        public void NextIcon()
        {
            _curPos++;
            if (_curPos == IconCollection.Count) _curPos = 0;
            OnPropertyChanged(nameof(CurIcon));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
