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

            public DragContent(TagGroup group, DragEventArgs e)
            {
                Group = group;
                Mode = GetDragMode(e);
                SourceTag = e.Data.GetData(typeof(TagWrap)) as TagWrap;                
            }
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
            var tagFrom = data.SourceTag;
            if (data.Mode == SwapMode.Copy)
                data.DestTag.Tag.Add(data.Nodes, data.DestNode, true);
            else
            {
                data.DestTag.Tag.Add(data.Nodes, data.DestNode, false);
                if (tagFrom != data.DestTag)
                    tagFrom.Tag.Remove(data.Nodes);
            }
        }

        private static bool FromGroupToTag(DragContent data)// вытащили из группы 
        {
            if (data.DestTag == null) return false;
            if (!data.Group.TagList.Contains(data.SourceTag)) return false;
            if (data.Group.TagList.Contains(data.DestTag)) return false;            
            if (data.Mode == SwapMode.Replace)
                data.Group.Remove(data.SourceTag);
            data.DestTag.SwapWith(data.SourceTag, data.Mode);
            return true;
        }

        private static void DragTag(DragContent data)
        {
            if (data.SourceTag == null) return;
            if (data.Mode == SwapMode.Swap || data.Group == null)
            {
                if (data.DestTag == null) return;
                data.DestTag.SwapWith(data.SourceTag, data.Mode);
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
            WorkSpace.Instance.FieldList.Remove(data.SourceField);
            var pos = WorkSpace.Instance.FieldList.IndexOf(data.DestField);
            WorkSpace.Instance.FieldList.Insert(pos, data.SourceField);
            WorkSpace.Instance.ActiveField = data.SourceField;
        }
        public static void DoDrag(DragEventArgs e)
        {
            DragContent data = e.Data.GetData(typeof(DragContent)) as DragContent;
            if (data == null) return;
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
