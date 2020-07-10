﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using System.Windows;
using SmartGrid.Items;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IContainer<T>
        {
            void Add(IEnumerable<T> items, T insertBefore);
            void Remove(T[] item);
        }
        
        public class DragElement<T> 
            where T : IHasHeader
        {
            private T[] _elements;
            private IContainer<T> _container;
            public T[] Elements => _elements;
            public IContainer<T> Container => _container;

            private IHasHeader CreateElement(IHasHeader proto)
            {
                string header = proto.Header.Header;
                if (typeof(T) == typeof(Node)) new Node(header);
                if (typeof(T) == typeof(Tag)) new Tag(header);
                if (typeof(T) == typeof(TagWrap)) new TagWrap(header);
                if (typeof(T) == typeof(SmartFiled)) new SmartFiled(header);
                throw new Exception($"Error Creating element {typeof(T)}");
            }
            public DragElement(T element, IContainer<T> container)
            {
                _elements = new []{element};
                _container = container;
            }
            public DragElement(IEnumerable<T> elements, IContainer<T> container)
            {
                _elements = elements.ToArray();
                _container = container;
            }
            public void Add(IHasHeader[] second)
            {
                var first = second.First();
                if (first.GetType() == typeof(T)) _container.Add(second as T[], _elements.First());
                else
                {
                    var added = (T)CreateElement(first);
                    added.Header.Header = first.Header.Header;
                    _container.Add(new[] {added}, _elements.First());
                }
            }

            public void Remove(T[] items)
            {
                _container.Remove(items);
            }
        }
        public class DragData<T1, T2> where T2 : IHasHeader where T1 : IHasHeader
        {
            public DragElement<T1> from;
            public DragElement<T2> to;
            private DragProcessor.SwapMode _mode;
            public DragProcessor.SwapMode Mode => _mode;

            public DragData(T1 element, IHasHeader contayner)
            {
                from = new DragElement<T1>(element, contayner as IContainer<T1>);
            }
            public void SetTarget(object sender, DragEventArgs e)
            {
                var d = new DragData<Node, T2>(new Node(""), null);
                _mode = GetDragMode(e);
                /*DragContent data = e.Data.GetData(typeof(DragContent)) as DragContent;
                if (data == null) return;
                var elementTo = sender as FrameworkElement;
                if (elementTo == null) return;
                data.DestField = elementTo.DataContext as SmartFiled;
                data.DestTag = elementTo.DataContext as TagWrap;
                data.DestNode = ((FrameworkElement)e.OriginalSource).DataContext as Node;
                if (data.Group == null) data.Group = elementTo.DataContext as TagGroup;

                if (data.DestField != null && data.Type != DargContentType.Field)
                    data.DestTag = data.DestField.WorkTag;*/

                //to = new DragElement<T2>(element, contayner);

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