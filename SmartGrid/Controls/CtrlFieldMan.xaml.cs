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
using SmartGrid.Drag;
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
        private void TxtHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            var _field = ((FrameworkElement) sender).DataContext as SmartFiled;
            if (_field == null) return;
            if (e.ClickCount == 2)
            {
                _field.IsEditMode = true;
                /*var txt = (TextBox)sender;
                txt.CaptureMouse();
                txt.MouseDown += (o, args) =>
                {
                    var second = o as TextBox;
                    if (o == null || o != txt)
                    {
                        _field.IsEditMode = false;
                        txt.ReleaseMouseCapture();
                    }
                };
                Mouse.AddPreviewMouseDownOutsideCapturedElementHandler((FrameworkElement)sender, OnMouseDownOutsideElement);*/
            }                
            else
                WorkSpace.Instance.ActiveField = _field;
            e.Handled = false;
        }
        private void TxtHeaderEditLostFocus(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as SmartFiled;
            if (item == null) return;
            item.IsEditMode = false;
        }

        private void BtnAddField_OnClick(object sender, RoutedEventArgs e)
        {
            var space = ((FrameworkElement) sender).DataContext as WorkSpace;
            if (space == null) return;
            var newItem = new SmartFiled();
            newItem.IsEditMode = true;
            newItem.Header = "Раздел";
            space.FieldList.Add(newItem);
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

        private void TxtHeader_OnDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(DragContent)) as DragContent;
            data.DestField = ((FrameworkElement) sender).DataContext as SmartFiled;
            DoDrag(e);
        }

        private void TxtHeader_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement) sender;
            _field = (SmartFiled)element.DataContext;
            DragHelper.SetClick(new DragContent(_field), e);
            e.Handled = false;
        }
        private void OnMouseDownOutsideElement(object sender, MouseButtonEventArgs e)
        {
            Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
            ReleaseMouseCapture();
            _field.IsEditMode = false;          
        }

        private void LstMain_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _field.IsEditMode = false;
        }


        private void TxtHeaderEdit_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var txt = (TextBox)sender;       
            txt.CaptureMouse();            
        }

        private void TxtHeaderPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            /*var second = sender as TextBox;
            if (second == null)
            {
                _field.IsEditMode = false;
            }*/
            
        }

        private void TxtHeader_OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            /*if (_field!=null)
                _field.IsEditMode = false;*/
        }

        private void TxtHeaderEdit_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter) return;
            var tb = (TextBox)sender;
            var item = (SmartFiled)tb.DataContext;
            tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            item.IsEditMode = false;
        }
    }
}
