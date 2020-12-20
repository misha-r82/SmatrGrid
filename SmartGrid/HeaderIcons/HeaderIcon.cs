using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using SmartGrid.Annotations;

namespace SmartGrid.HeaderIcons
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(System.Array))]  //here !!!!! for System.Byte[*]
    public class IconElement
    {
        [DataMember] private byte[] binData;
        [DataMember] public string Name { get; set; }
        [DataMember] public IconCollection Collection { get; private set; }
        private IconElement()
        { }

        public IconElement(string name, IconCollection collection)
        {
            Name = name;
            Collection = collection;
        }


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

  /*    [DataContract(IsReference = true)]
  public class HeaderIcon : INotifyPropertyChanged
    {
        [DataMember] private IconElement _icon;
        public IconElement Icon => _icon;
        [DataMember]public HeaderIcon Parent { get; private set; }
        [DataMember] public IconCollection Collection { get; private set; }
        private HeaderIcon()
        {
            _icon = new IconElement("Base Icon");
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

        public HeaderIcon ToIconSet()
        {
            var item = new HeaderIcon();
            item._icon = _icon;
            item.Collection = new IconCollection(Collection);
            foreach (var icon in item.Collection)
                icon.Parent = item;
            item.Collection[0] = item;
            return item;
        }
        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if(Collection.Count == 0) Collection.Add(this);
        }
        public bool IsSameIconData(HeaderIcon second)
        {
            return second.Icon == Icon;
        }
        public HeaderIcon NextIcon()
        {
            if (Parent != null) return Parent.Collection.NextIcon(this);
            return Collection.NextIcon(this);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }*/
}
