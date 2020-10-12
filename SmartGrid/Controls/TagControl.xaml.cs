using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Lib;
using SmartGrid.Annotations;
using SmartGrid.Controls;
using SmartGrid.Drag;
using SmartGrid.Undo;

namespace SmartGrid
{
    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    public partial class TagControl : UserControl, INotifyPropertyChanged
    {
        public TagControl()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) =>
            {
                CurTag = DataContext as Tag;
                NewNode = new Node();
                gridNewNode.DataContext = NewNode;
            };
            panelTools.DataContext = this;

        }
        public Node NewNode { get; private set; }
        
        private HeaderUndoScope<Tag> _headerUndoScope;
        private Node[] SelectedNodes => lstMain.SelectedItems.OfType<Node>().ToArray();

        private Tag CurTag { get; set; }
        private void AddNew()
        {
            CurTag.Add(NewNode.GetClone());
            NewNode.Header.Header = "";
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            AddNew();
        }
        private void txtNewNode_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!Sett.Settings.CtrlEnter) AddNew();
                else
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
                    AddNew();                               
            }
        }
        private void On_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(DragProcessor.DragElement)) as DragProcessor.DragElement;
            if (data == null || data.FirstElement == null) return;
            if (data.FirstElement.GetType() == typeof(Node))
            {
                DragProcessor.DoDrag(sender, e);
                e.Handled = true;
            }
        }
        private void Node_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var nodeCtrl = sender as NodeCotrol;
            if (nodeCtrl != null && nodeCtrl.IsDraEnable)
            {
                var nodes = new List<Node>(SelectedNodes);
                var node = ((FrameworkElement) sender).DataContext as Node;
                if (!nodes.Contains(node)) nodes.Add(node);
                var data = new DragProcessor.DragElement( nodes, CurTag);
                DragHelper.SetClick(data, e);
                e.Handled = false;
            }
        }
        private void CommDell_Exec(object sender, ExecutedRoutedEventArgs e)
        {
            CurTag.Remove(SelectedNodes);
        }
        private void BtnLink_OnClick(object sender, RoutedEventArgs e)
        {
            Editor.NodeEditor.HyperlinkCmd();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CtrlHeader_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _headerUndoScope = new HeaderUndoScope<Tag>(CurTag, "Изменение заголовка набора {0}");
        }
        private void CtrlHeader_OnLostFocus(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.AddScope(_headerUndoScope);
        }
        private void LstMain_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WorkSpace.Instance.Curent.SetSelectedElements(lstMain.SelectedItems);
        }

        private void GridMain_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsFocused) Focus();

        }

        private void TagControl_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (IsFocused) ctrlHeader.Focus();
        }
    }
}
