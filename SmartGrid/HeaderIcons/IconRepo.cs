using System.Net.Mime;
using System.Security.Permissions;
using System.Windows;
using Microsoft.Win32;

namespace SmartGrid.HeaderIcons
{
    public class IconRepo
    {
        private static string ICONS_FILE = "icons.xml";
        private static IconsGroup _repo;
        private static string FileName => System.AppDomain.CurrentDomain.BaseDirectory + ICONS_FILE;

        public static IconsGroup CoreGroup
        {
            get
            {
                if (_repo == null) _repo = Lib.FileIO.DeserializeDataContract<IconsGroup>(FileName);
                if (_repo == null) _repo = new IconsGroup();
                return _repo;
            }
        }

        public static void Save()
        {
            Lib.FileIO.SerializeDataContract(CoreGroup, FileName);
        }
    }
}