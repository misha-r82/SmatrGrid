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
    public class IconCollection
    {
        public IconCollection(HeaderIcon firstIcon)
        {
            Icons = new List<HeaderIcon>();
            Icons.Add(firstIcon);
        }
        public List<HeaderIcon> Icons { get; private set; }
    }
    [DataContract(IsReference = true)]
    public class HeaderIcon : INotifyPropertyChanged
    {
        [DataMember]
        private readonly IconData _icon;
        public IconData Icon => _icon;
        public HeaderIcon Parent { get; private set; }
        public IconCollection Collection { get; private set; }
        private HeaderIcon()
        {
            _icon = new IconData("Base Icon");
            Collection = new IconCollection(this);          
        }
        public static HeaderIcon CreateBaseItem() { return new HeaderIcon();}
        public HeaderIcon Create(HeaderIcon parent, string name, Stream stram)
        {
            var newItem = new HeaderIcon();
            newItem.Icon.Name = name;
            newItem.Icon.FromStream(stram);
            newItem.Parent = parent;
            Collection.Add(newItem);
            OnPropertyChanged(nameof(Collection));
            return newItem;
        }

        public void RemoveFromItemCollection(HeaderIcon icon)
        {
            Collection.Remove(icon);
            OnPropertyChanged(nameof(IconCollection));
        }
        //[OnDeserialized()]
        //internal void OnDeserializedMethod(StreamingContext context)
        //{
        //    if(IconCollection.Count == 0) IconCollection.Add(this);
        //}
        public HeaderIcon NextIcon()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
