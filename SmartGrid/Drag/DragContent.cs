using System;
using System.Collections.Generic;
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
                var tag2 = second as Tag;
                if (tag2 == null) tag2 = new Tag(second.Header.Header);
                tWrap.Tag = tag2;
            }
        }
        public class TagGroupElement : IDraElement
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
        }
        //public abstract class DragElement
        //{
        //    private IHasHeader _coteiner;
        //    private IHasHeader _element;

        //    public DragElement(IHasHeader element)
        //    {
        //        _element = element;
        //    }

        //    public void DragToMe(IHasHeader element)
        //    {
        //       /* var t = element.GetType().;
        //        Convert.
        //        var contayner = Convert.ChangeType(_element, HeaderableList<t>)  */
        //    }
        //    public void DragToMe(IEnumerable<IHasHeader> elements)
        //    {
                
        //    }

        //    public void Remove(IEnumerable<IHasHeader> elements)
        //    {

        //    }

        //}



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