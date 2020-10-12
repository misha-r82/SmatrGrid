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
            var arr = elements.ToArray();
            Editor.NodeEditor.SaveToNodeVal();
            string serialized = "";
            if (arr[0] is Node)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<Node>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Nodes.ToString(), serialized);
            } else 
            if(arr[0] is Tag)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<Tag>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Tag.ToString(), serialized);
            }else if (arr[0] is SmartFiled)
            {
                serialized = FileIO.SerializeDataContract(arr.OfType<SmartFiled>().ToArray());
                Clipboard.SetData(DragProcessor.DargContentType.Field.ToString(), serialized);
            }
            
        }
        private void CommandCopy_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandCut_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CopyNodesToClipboard(WorkSpace.Instance.Curent.SelectedElements);
            var contayiner = WorkSpace.Instance.Curent.Contayner as DragProcessor.IContainer;
            contayiner?.Remove(WorkSpace.Instance.Curent.SelectedElements);
        }

        private void CommandPaste_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IHasHeader[] elements = null;
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Nodes.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Nodes.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<Node[]>(serialized).Cast<IHasHeader>().ToArray();
            } else
            if (Clipboard.ContainsData(DragProcessor.DargContentType.Tag.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Tag.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<Tag[]>(serialized).Cast<IHasHeader>().ToArray();
            } else if (Clipboard.ContainsData(DragProcessor.DargContentType.Field.ToString()))
            {
                var serialized = Clipboard.GetData(DragProcessor.DargContentType.Field.ToString()).ToString();
                elements = FileIO.DeserializeXMLFromString<SmartFiled[]>(serialized).Cast<IHasHeader>().ToArray();
            }
            var contayiner = WorkSpace.Instance.Curent.Contayner as DragProcessor.IContainer;
            contayiner?.Add(elements);
        }
    }
}
