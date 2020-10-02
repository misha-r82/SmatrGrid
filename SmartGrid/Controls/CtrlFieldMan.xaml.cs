using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.Windows.Threading;
using Lib.ColorPiecker;
using SmartGrid.Controls;
using SmartGrid.Drag;
using SmartGrid.Undo;
using static SmartGrid.DragProcessor;

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for CtrlFieldMan.xaml
    /// </summary>
    public partial class CtrlFieldMan : UserControl
    {
        public CtrlFieldMan()
        {
            InitializeComponent();
        }
        private SmartFiled _field;
        private HeaderUndoScope<SmartFiled> _headerScope;
        private void BtnAddField_OnClick(object sender, RoutedEventArgs e)
        {
            var space = ((FrameworkElement) sender).DataContext as WorkSpace;
            if (space == null) return;
            var newItem = new SmartFiled();
            newItem.IsEditMode = true;
            newItem.Header.Header = "Раздел";
            space.Add(newItem);
        }

        private void CommandDelete_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var list = lstMain.DataContext as IList<SmartFiled>;
            var items = lstMain.SelectedItems.OfType<SmartFiled>().ToArray();
            if (MessageBox.Show("Действительно удалить выбранные элементы?", "Подтверждение удаления", 
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                foreach (var item in items)
                    list.Remove(item);
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            DoDrag(sender, e);
        }

        private void TxtHeader_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement) sender;
            _field = (SmartFiled)element.DataContext;
            DragHelper.SetClick(new DragElement((IDragElement) _field, WorkSpace.Instance), e);
            e.Handled = false;
        }


        private void ItmColor_OnClick(object sender, RoutedEventArgs e)
        {
            var f = new ColorPickerDialog();
            f.ShowDialog();
        }

        private void CtrlHeader_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _headerScope= new HeaderUndoScope<SmartFiled>(_field, "Изменение заголовка раздела {0}");
        }

        private void CtrlHeader_OnLostFocus(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.AddScope(_headerScope);
        }
    }
}
