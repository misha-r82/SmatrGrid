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
            if (e.ClickCount < 2 || e.ChangedButton != MouseButton.Left) return;
            var icon = ((FrameworkElement) sender).DataContext as HeaderIcon;
            if (icon != null) _header.Header.Icons.NextIcon(icon);
        }

        private void HeaderCtrl_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _header = e.NewValue as IHasHeader;
        }

        private void IocnMenu_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var iconElement = sender as FrameworkElement;
            var menu = iconElement.ContextMenu;
            var icon = (HeaderIcon) iconElement.DataContext;
            if (icon.Parent != null) icon = icon.Parent;
            var menuData = new List<MenuDataItem>();
            foreach (var headerIcon in icon.Collection)
            {
                var command = new SetIconCommand(headerIcon, _header.Header.Icons);
                menuData.Add(new MenuDataItem(headerIcon.Icon.Name, headerIcon.Icon.IconBitMap, command));
            }
            menu.DataContext = menuData;
        }
        public class SetIconCommand : ICommand
        {
            private HeaderIcon _icon;
            private IconSet _iconSet;
            public SetIconCommand(HeaderIcon icon, IconSet iconSet)
            {
                _icon = icon;
                _iconSet = iconSet;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                _iconSet.Add(_icon);
            }
            public event EventHandler CanExecuteChanged;
        }
    }
}
