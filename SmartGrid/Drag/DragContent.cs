using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.RightsManagement;
using SmartGrid.Items;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IDraElement
        {
            void DragToMe(IHasHeader second);
        }
        public interface IContainer<T>
        {
            void Add(T[] items, T insertBefore);
            void Remove(T[] item);
        }

        public class DragElement<T> 
            where T : IHasHeader, new()

        {
            private T[] _elements;
            private IContainer<T> _container;
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
                    var added = new T();
                    added.Header.Header = first.Header.Header;
                    _container.Add(new[] {added}, _elements.First());
                }
            }

            public void Remove(T[] items)
            {
                _container.Remove(items);
            }
        }
        public class NodeElement : IDraElement
        {
            private Node _node;
            private Tag _tag;
            public NodeElement(Node node, Tag tag)
            {
                _node = node;
                _tag = tag;
            }

            public Node Node => _node;

            public Tag Tag => _tag;

            public void DragToMe(IHasHeader second)
            {
                var node2 = second as Node;
                if (node2 == null) node2 = new Node(second.Header.Header);
                _tag.Add(node2, _node);
            }
        }
        public class TagElement : IDraElement
        {
            private readonly TagWrap tWrap;

            public TagElement(TagWrap tWrap)
            {
                this.tWrap = tWrap;
            }
            public TagWrap TWrap => tWrap;

            public void DragToMe(IHasHeader second)
            {
                var tag2 = second as TagWrap;
                if (tag2 == null) tag2 = new TagWrap(second.Header.Header);
                tWrap.Tag = tag2.Tag;
            }
        }
        /*public class TagGroupElement : IDraElement
        {
            private readonly TagGroup _tagGroup;

            public TagGroupElement(TagGroup tagGroup)
            {
                this._tagGroup = tagGroup;
            }

            public TagGroup TagGroup => _tagGroup;

            public void DragToMe(IHasHeader second)
            {
                var tag2 = second as TagWrap;
                if (tag2 == null) tag2 = new TagWrap(second.Header.Header);
                tWrap.Tag = tag2;
            }
        }*/

        public class DragData<T1, T2> where T2 : IHasHeader, new() where T1 : IHasHeader, new()
        {
            public DragElement<T1> from;
            public DragElement<T2> to;

            DragData(T1 element, IContainer<T1> contayner)
            {
                @from = new DragElement<T1>(element, contayner);
            }

            public void SetTarget(T2 element, IContainer<T2> contayner)
            {
                to = new DragElement<T2>(element, contayner);
            }
        }



        public class DragContent
        {
            public SwapMode Mode;
            public DargContentType Type;
            public TagWrap SourceTag;
            public TagWrap DestTag;
            public TagGroup Group;
            public IEnumerable<Node> Nodes;
            public Node DestNode;
            public SmartFiled SourceField;
            public SmartFiled DestField;

            public DragContent(TagWrap sourceTag)
            {
                Type = DargContentType.Tag;
                SourceTag = sourceTag;
            }

            public DragContent(IEnumerable<Node> nodes, TagWrap sourceTag)
            {
                Type = DargContentType.Nodes;
                Nodes = nodes;
                SourceTag = sourceTag;

            }
            public DragContent(SmartFiled sourceField)
            {
                Type = DargContentType.Field;
                SourceField = sourceField;
            }

            public DragContent(IEnumerable<IHasHeader> elements, IHasHeader conteier)
            {

            }
        }
    }
}