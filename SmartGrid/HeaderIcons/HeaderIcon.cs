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
    [DataContract(IsReference = true)]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(System.Array))]  //here !!!!! for System.Byte[*]
    public class HeaderIcon : INotifyPropertyChanged
    {
        private HeaderIcon(Stream stream): this()
        {
            FromStream(stream);
        }
        private HeaderIcon()
        {
            IconCollection = new ObservableCollection<HeaderIcon>(new List<HeaderIcon>());
        }
        [DataMember]
        private byte[] binData;
        [DataMember]
        public string Name { get; set; }
        public BitmapImage Icon { get; private set; }
        [DataMember]
        public ObservableCollection<HeaderIcon> IconCollection { get; private set; }
        [DataMember] public HeaderIcon Parent { get; private set; }
        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (binData == null) return;
            var stream = new MemoryStream(binData);
            FromStream(stream);
        }

        public static HeaderIcon CreateBaseItem()
        {
            return new HeaderIcon(){Name = "BaseItem"};
        }

        public HeaderIcon CreateChield(string name = "", Stream stream = null)
        {
            return new HeaderIcon(stream){Name =name, Parent = this};
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
                Icon = bitmap;
                OnPropertyChanged(nameof(Icon));
            }
            catch (Exception e)
            {
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
