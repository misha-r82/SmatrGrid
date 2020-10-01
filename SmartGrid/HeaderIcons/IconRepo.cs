using System.Net.Mime;
using System.Security.Permissions;
using System.Windows;
using Microsoft.Win32;

namespace SmartGrid.HeaderIcons
{
    public class IconRepo
    {
        private static string ICONS_FILE = "icons.xml";
        private static HeaderIcon _repo;
        private static string FileName => System.AppDomain.CurrentDomain.BaseDirectory + ICONS_FILE;

        public static HeaderIcon CoreIcon
        {
            get
            {
                if (_repo == null) _repo = Lib.FileIO.DeserializeDataContract<HeaderIcon>(FileName);
                if (_repo == null) _repo = HeaderIcon.CreateBaseItem();
                return _repo;
            }
        }

        public static void Save()
        {
            Lib.FileIO.SerializeDataContract(CoreIcon, FileName);
        }
    }
}