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
        public MenuDataItem(string name, BitmapImage image, ICommand command)
        {
            Name = name;
            Icon = image;
            Command = command;
        }
        public  string Name { get;}
        public BitmapImage Icon { get;}
        public Image Img => new Image() {Source = Icon};
        public ICommand Command { get; }
        public string ToolTip { get; set; }
    }
}
