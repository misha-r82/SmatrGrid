using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lib.UI
{
    public class MenuDataItem
    {
        public MenuDataItem(string name, BitmapImage image)
        {
            Name = name;
            Icon = image;
        }
        public  string Name { get; set; }
        public BitmapImage Icon { get; set; }
        public Image Img => new Image() {Source = Icon};
        public ICommand Command { get; set; }
        public string ToolTip { get; set; }
    }
}
