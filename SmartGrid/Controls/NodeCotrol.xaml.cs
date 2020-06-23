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
using SmartGrid.Undo;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для NodeCotrol.xaml
    /// </summary>
    public partial class NodeCotrol : UserControl
    {
        private HeaderUndoScope<Node> _headerScope;
        //private NodeContentScope _contentScope;
        public NodeCotrol()
        {
            InitializeComponent();
        }
        private bool isBodyEditionMode;
        public bool IsBodyEditionMode
        {
            get { return txtBody.IsFocused; }
        }

        public bool IsDraEnable
        {
            get => !(IsBodyEditionMode || ctrlHeader.IsEditing);
        }
        private void TxtVal_OnGotFocus(object sender, RoutedEventArgs e)
        {
            SetEditorRtb(sender);
        }

        private void TxtVal_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetEditorRtb(sender);
        }
        private void TxtVal_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var tb = sender as RichTextBox;
            if (tb == null) return;
            tb.Document.PageWidth = tb.ActualWidth;
        }
        private void SetEditorRtb(object rtb)
        {
            var tb = rtb as RichTextBox;
            if (tb == null) return;
            Editor.NodeEditor.Rtb = tb;
        }
        private void FrameworkElement_OnInitialized(object sender, EventArgs e)
        {
            SetEditorRtb(sender);
        }

        private void CommandCopy_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsBodyEditionMode)
                Editor.NodeEditor.Rtb.Copy();
        }

        private void CommandCut_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsBodyEditionMode)
                Editor.NodeEditor.Rtb.Cut();
        }

        private void CommandPaste_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsBodyEditionMode)
                Editor.NodeEditor.Rtb.Paste();
        }

        private void NodeCotrol_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var node = DataContext as Node;
            _headerScope = new HeaderUndoScope<Node>(node, "Изменение заголовка записи {0}");

        }

        private void NodeCotrol_OnLostFocus(object sender, RoutedEventArgs e)
        {
            WorkSpace.Instance.Undo.AddScope(_headerScope);

        }
    }
}
