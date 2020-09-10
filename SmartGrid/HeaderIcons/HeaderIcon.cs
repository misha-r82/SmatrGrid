using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartGrid.HeaderIcons
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(byte[]))]
    [KnownType(typeof(System.Array))]  //here !!!!! for System.Byte[*]
    public class HeaderIcon
    {
        public HeaderIcon(Stream stream)
        {
            FromStream(stream);
        }
        public HeaderIcon()
        {
            IconCollection = new ObservableCollection<HeaderIcon>(new List<HeaderIcon>());
        }
        [DataMember]
        private byte[] binData;
        [DataMember]
        public string Name { get; set; }
        public BitmapImage Icon { get; private set; }
        [DataMember]
        public ObservableCollection<HeaderIcon> IconCollection { get; }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var stream = new MemoryStream(binData);
            FromStream(stream);
        }

        private void FromStream(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                binData = br.ReadBytes((int) stream.Length);
            }
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(binData);
            //bitmap.UriSource = new Uri(@"C:\Users\misha\RiderProjects\SmartGrid\smartgrid\SmartGrid\img\cut.png", UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            Icon = bitmap;
            // bitmap.Freeze();

        }
    }
}
