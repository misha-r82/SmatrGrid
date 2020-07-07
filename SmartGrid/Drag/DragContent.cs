using System;
using System.Collections.Generic;
using System.Security.RightsManagement;

namespace SmartGrid
{
    public partial class DragProcessor
    {
        public interface IDraElement
        {
            void DragToMe(IDraElement second);
        } 
        private static class ElementFactory
        {
            public static IHasHeader CreateElement(IHasHeader prototype)
            {
                var type = prototype.GetType();
                if (type == typeof(Node)) return new Node(prototype.Header.Header);
                if (type == typeof(Tag)) return new Tag(prototype.Header.Header);
                throw new Exception("Ошибка создания элемента");
            }
        }
        public class DragElement
        {
            private IHasHeader _coteiner;
            private IHasHeader _element;

            public DragElement(IHasHeader element)
            {
                _element = element;
            }
            public void DragToMe(IEnumerable<IHasHeader> elements)
            {
                
            }

            public void Remove(IEnumerable<IHasHeader> elements)
            {

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