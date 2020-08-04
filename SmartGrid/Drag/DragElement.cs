using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Windows.Forms;
using SmartGrid.Items;
using Test;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IContainer : IDragElement
        {
            void Add(IEnumerable<IHasHeader> items, IHasHeader insertBefore = null);
            void Remove(IEnumerable<IHasHeader> items);
            bool AcceptType(Type type);
        }

        public interface IDragElement : IHasHeader, ICloneableEx<IDragElement>
        { }

        public class DragElement
        {
            private IDragElement[] _elements;
            private IContainer _container;
            public IDragElement[] Elements => _elements;
            public IContainer Container => _container;
            public IDragElement FirstElement => _elements.First();

            private IHasHeader CreateElement(Type type, string header)
            {
                
                if (type == typeof(Node)) return new Node(header);
                if (type == typeof(Tag)) return new Tag(header);
                if (type == typeof(SmartFiled)) return new SmartFiled(header);
                throw new Exception($"Error Creating element {type}");
            }

            public DragElement(IEnumerable<IDragElement> elements, IDragElement container)
            {
                _elements = elements.ToArray();
                _container = container as IContainer;
            }
            public DragElement(IDragElement element, IDragElement container) : this(new []{element}, container)
            { }


            public void Add(IEnumerable<IDragElement> second)
            {
                var secondFirst = second.First();
                if (_container.AcceptType(secondFirst.GetType())) _container.Add(second, FirstElement);
                else
                {
                    var added = CreateElement(FirstElement.GetType(), secondFirst.Header.Header);
                    _container.Add(new[] {added}, FirstElement);
                }
            }

            public void Remove(IEnumerable<IDragElement> items)
            {
                _container.Remove(items);
            }
        }
    }
}