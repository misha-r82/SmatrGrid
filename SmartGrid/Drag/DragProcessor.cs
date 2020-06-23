using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Lib;

namespace SmartGrid
{
    public class DragProcessor
    {
        public enum DargContentType { Tag, TagToGroup, Nodes, Field}
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
        }

        public enum SwapMode { Swap, Copy, Replace }
        public static SwapMode GetDragMode(DragEventArgs e)
        {
            var mode = SwapMode.Replace;
            switch (e.KeyStates)
            {
                case DragDropKeyStates.ControlKey: mode = SwapMode.Copy; break;
                case DragDropKeyStates.ShiftKey: mode = SwapMode.Swap; break;
            }
            return mode;
        }

        private static void DragNodes(DragContent data)
        {
            if (data.Nodes == null || !data.Nodes.Any()) return;
            if (data.Group != null)
            {
                foreach (var node in data.Nodes)
                {
                   var newTag = new TagWrap(new Tag(node.Header.Header));
                   data.Group.Add(new []{newTag});
                   if (data.Mode == SwapMode.Replace) data.SourceTag.Tag.Remove(node);               
                }
                return;
            }

            if (data.DestField != null)
            {
                foreach (var node in data.Nodes)
                {
                    var newField = new SmartFiled(node.Header.Header);
                    WorkSpace.Instance.FieldList.Add(newField);
                    if (data.Mode == SwapMode.Replace) data.SourceTag.Tag.Remove(node);
                }
                return;
            }
            var tagFrom = data.SourceTag;
            var tagTo = data.DestTag;
            if (data.Mode == SwapMode.Copy)
                tagTo.Tag.Add(data.Nodes, data.DestNode, true);
            else
            {
                if (tagTo == tagFrom) tagFrom.Tag.Remove(data.Nodes);
                    tagTo.Tag.Add(data.Nodes, data.DestNode, false);
                if (tagTo != tagFrom)
                    tagFrom.Tag.Remove(data.Nodes);
                if (data.Mode == SwapMode.Swap && data.DestNode != null)
                {
                    tagFrom.Tag.Add(data.DestNode, data.Nodes.First());
                    tagTo.Tag.Remove(data.DestNode);
                }
                    
            }

        }
        public static void SwapTagWith(TagWrap first, TagWrap second, SwapMode mode)
        {
            if (second == null) second = new TagWrap();
            if (object.ReferenceEquals(first, second)) return;
            switch (mode)
            {
                case SwapMode.Copy:
                    first.Tag = second.Tag.GetClone(first); break;
                case SwapMode.Swap:
                    var tmpTag = second.Tag;
                    second.Tag = first.Tag;
                    first.Tag = tmpTag; break;
                default:
                    first.Tag = second.Tag;
                    second.Tag = new Tag(); break;
            }

        }
        private static bool FromGroupToTag(DragContent data)// вытащили из группы 
        {
            if (data.DestTag == null) return false;
            if (!data.Group.TagList.Contains(data.SourceTag)) return false;
            if (data.Group.TagList.Contains(data.DestTag)) return false;            
            if (data.Mode == SwapMode.Replace)
                data.Group.Remove(data.SourceTag);
            SwapTagWith(data.DestTag, data.SourceTag, data.Mode);
            return true;
        }

        private static void DragTag(DragContent data)
        {

            if (data.SourceTag == null) return;
            if (data.DestField != null)
            {
                var newField = new SmartFiled(data.SourceTag.Header.Header);
                WorkSpace.Instance.FieldList.Add(newField);
                if (data.Mode == SwapMode.Replace)
                    if(data.Group != null)
                        data.Group.Remove(data.SourceTag);
                    else data.SourceTag.Tag = new Tag();
                return;
            }
            if (data.Mode == SwapMode.Swap || data.Group == null)
            {
                if (data.DestTag == null) return;
                SwapTagWith(data.DestTag, data.SourceTag, data.Mode);
                return;
            }
            if (FromGroupToTag(data)) return;
            // нужно добавлять в группу
            int pos = data.DestTag == null ? -1 : data.Group.TagList.IndexOf(data.DestTag);
            if (pos == -1) pos = data.Group.TagList.Count;
            var added = data.SourceTag.GetClone();
            data.Group.TagList.Insert(pos, added);
            if (data.Mode == SwapMode.Replace)
                if (data.DestTag != null && data.Group.TagList.Contains(data.SourceTag))
                    data.Group.Remove(data.SourceTag);
                else
                    data.SourceTag.Tag = new Tag();
        }
        private static void DragToField(DragContent data)
        {
            if (data.SourceField == data.DestField) return;
            if (data.DestTag != null)
            {
                var newTag = new Tag(data.SourceField.Header.Header);
                data.DestTag.Tag = newTag;
                if (data.Mode == SwapMode.Replace) WorkSpace.Instance.Remove(data.SourceField);
                return;
            }

            if (data.Group != null)
            {
                var newTag = new TagWrap(new Tag(data.SourceField.Header.Header));
                data.Group.Add(new[] { newTag });
                if (data.Mode == SwapMode.Replace) WorkSpace.Instance.Remove(data.SourceField);
                return;
            }
            WorkSpace.Instance.FieldList.Remove(data.SourceField);
            var pos = WorkSpace.Instance.FieldList.IndexOf(data.DestField);
            WorkSpace.Instance.FieldList.Insert(pos, data.SourceField);
            WorkSpace.Instance.ActiveField = data.SourceField;
        }
        public static void DoDrag(object sender, DragEventArgs e)
        {
            DragContent data = e.Data.GetData(typeof(DragContent)) as DragContent;
            if (data == null) return;
            var elementTo = sender as FrameworkElement;
            if (elementTo == null) return;
            data.DestField = elementTo.DataContext as SmartFiled;
            data.DestTag = elementTo.DataContext as TagWrap;
            data.DestNode = ((FrameworkElement) e.OriginalSource).DataContext as Node;
            if (data.Group == null) data.Group = elementTo.DataContext as TagGroup;
            
            if (data.DestField != null && data.Type != DargContentType.Field)
                data.DestTag = data.DestField.WorkTag;
            data.Mode = GetDragMode(e);
            switch (data.Type)
            {
                case DargContentType.Tag:
                    DragTag(data); break;
                case DargContentType.Nodes:
                    DragNodes(data); break;
                case DargContentType.Field:
                    DragToField(data); break;
            }
        }
    }
}
