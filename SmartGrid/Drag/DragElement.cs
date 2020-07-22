using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Windows.Forms;
using SmartGrid.Items;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IContainer
        {
            void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null);
            void Remove(IEnumerable<IHasHeader> item);
            bool AcceptType(Type type);
        }
        
        public class DragElement
        {
            private IHasHeader[] _elements;
            private IContainer _container;
            public IHasHeader[] Elements => _elements;
            public IContainer Container => _container;
            public IHasHeader FirstElement => _elements.First();

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
                var secondFirst = second.First();
                if (_container.AcceptType(secondFirst.GetType())) _container.Add(second, FirstElement);
                else
                {
                    var added = CreateElement(FirstElement.GetType(), secondFirst.Header.Header);
                    _container.Add(new[] {added}, FirstElement);
                }
            }

            public void Remove(IEnumerable<IHasHeader> items)
            {
                _container.Remove(items);
            }
        }
    }
}