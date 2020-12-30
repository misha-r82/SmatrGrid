using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib.UI;
using SmartGrid.HeaderIcons;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для HeaderCtrl.xaml
    /// </summary>
    public partial class CtrlHeaderIcons : UserControl
    {
        
        private IHasHeader _header;
        private IconElement _curIcon;
        public CtrlHeaderIcons()
        {
            InitializeComponent();
        }

        public bool IsEditing
        {
            get { return txtHeader.IsEditing; }
        }

        private void Icon_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _curIcon  = ((FrameworkElement)sender).DataContext as IconElement;
            if (e.ClickCount < 2 || e.ChangedButton != MouseButton.Left) return;
            if (_curIcon != null) _header.Header.Icons.NextIcon(_curIcon);
        }

        private void HeaderCtrl_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _header = e.NewValue as IHasHeader;
        }

        private void IocnMenu_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var iconElement = sender as FrameworkElement;
            var menu = iconElement.ContextMenu;
            var menuData = new List<MenuDataItem>();
            foreach (var headerIcon in _curIcon.Collection)
            {
                var command = new HeaderIconCommands.SetIconCommand(headerIcon, _header.Header.Icons);
                menuData.Add(new MenuDataItem(headerIcon.Name, headerIcon.IconBitMap, command));
            }

            var delComm = ApplicationCommands.Delete;
            menuData.Add(new MenuDataItem("Удалить", new BitmapImage(new Uri(@"pack://application:,,,/img/delete.png")), delComm));
            menu.DataContext = menuData;
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _header.Header.Icons.Remove(_curIcon);
        }
    }
}
