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
using Lib;

namespace SmartGrid.Controls
{
    /// <summary>
    /// Логика взаимодействия для CtrlCopyPaste.xaml
    /// </summary>
    public partial class CtrlCopyPaste : UserControl
    {
        public CtrlCopyPaste()
        {
            InitializeComponent();
        }
        private void CopyNodesToClipboard(IEnumerable<IHasHeader> elements)
        {
            if (elements == null || !elements.Any()) return;
            if (elements.First() is Node)
                Editor.NodeEditor.SaveToNodeVal();
            var serialized = FileIO.SerializeJson(elements.ToArray());
            Clipboard.SetData(typeof(IHasHeader[]).ToString(), serialized);
        }
        private void CommandCopy_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandCut_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
            var contayiner = WorkSpace.Instance.Curent.Element as DragProcessor.IContainer;
            contayiner?.Remove(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandPaste_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Nodes.ToString()))
            {
                var serialized = Clipboard.GetData(typeof(IHasHeader[]).ToString()).ToString();
                var elements = FileIO.DeserializeJsonFromString<IHasHeader[]>(serialized);
                var contayiner = WorkSpace.Instance.Curent.Element as DragProcessor.IContainer;
                contayiner?.Add(elements);
            }
        }
    }
}
