using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Forms;
using SmartGrid.Items;
using DragEventArgs = System.Windows.DragEventArgs;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IContainer
        {
            void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore);
            void Remove(IEnumerable<IHasHeader> item);
        }
        
        public class DragElement
        {
            private IHasHeader[] _elements;
            private IContainer _container;
            public IHasHeader[] Elements => _elements;
            public IContainer Container => _container;

            private IHasHeader CreateElement(Type type, string header)
            {
                
                if (type == typeof(Node)) return new Node(header);
                if (type == typeof(Tag)) return new Tag(header);
                if (type == typeof(TagWrap)) return new TagWrap(header);
                if (type == typeof(SmartFiled)) return new SmartFiled(header);
                throw new Exception($"Error Creating element {type}");
            }

            public DragElement(IEnumerable<IHasHeader> elements, IHasHeader container)
            {
                _elements = elements.ToArray();
                _container = container as IContainer;
            }
            public DragElement(IHasHeader element, IHasHeader container) : this(new []{element}, container)
            { }

            public void Add(IHasHeader[] second)
            {
                var firstElement = second.First();
                if (firstElement.GetType() == second.GetType()) _container.Add(second, _elements.First());
                else
                {
                    var added = CreateElement(firstElement.GetType(), firstElement.Header.Header);
                    added.Header.Header = firstElement.Header.Header;
                    _container.Add(new[] {added}, _elements.First());
                }
            }

            public void Remove(IEnumerable<IHasHeader> items)
            {
                _container.Remove(items);
            }
        }
        public class DragData
        {
            public DragElement from;
            public DragElement to;
            private DragProcessor.SwapMode _mode;
            public SwapMode Mode => _mode;
            public DragData(object sender, DragEventArgs e)
            {
                from = e.Data.GetData(typeof(DragElement)) as DragElement;
                if (from == null) throw new ArgumentException("Drag incorect type from");
                _mode = GetDragMode(e);
                to =new DragElement(((FrameworkElement) sender).DataContext as IHasHeader,
                                    ((FrameworkElement) e.OriginalSource).DataContext as IHasHeader);
            }
        }

      /*  public class DragContent
        {
            public DragProcessor.SwapMode Mode;
            public DragProcessor.DargContentType Type;
            public TagWrap SourceTag;
            public TagWrap DestTag;
            public TagGroup Group;
            public IEnumerable<Node> Nodes;
            public Node DestNode;
            public SmartFiled SourceField;
            public SmartFiled DestField;

            public DragContent(TagWrap sourceTag)
            {
                Type = DragProcessor.DargContentType.Tag;
                SourceTag = sourceTag;
            }

            public DragContent(IEnumerable<Node> nodes, TagWrap sourceTag)
            {
                Type = DragProcessor.DargContentType.Nodes;
                Nodes = nodes;
                SourceTag = sourceTag;

            }
            public DragContent(SmartFiled sourceField)
            {
                Type = DragProcessor.DargContentType.Field;
                SourceField = sourceField;
            }

        }*/
    }
}