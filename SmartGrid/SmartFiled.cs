using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SmartGrid.Annotations;

namespace SmartGrid
{
    [DataContract]
    class SmartFiled : INotifyPropertyChanged
    {
        public TagWrap[,] Cells;
        [DataMember] public TagWrap WorkTag { get; set;}
        [DataMember] public TagGroup TagGrp { get; private set; }
        [DataMember] public GridWidth GridWidth { get; private set; }
        public SmartFiled()
        {
            TagGrp = new TagGroup();
            GridWidth = new GridWidth();
            TagGrp.TagList.Add(new TagWrap("Корзина"));
            TagGrp.TagList.Add(new TagWrap("Важное"));
            Cells = new TagWrap[3,3];
            for (int i = 0; i<Cells.GetLength(0); i++)
                for (int j = 0; j < Cells.GetLength(1); j++)
                {
                    Cells[i, j] = new TagWrap(i.ToString() + j);
                }
            foreach (var node in Cells[0,0].Tag)
            {
                node.ViewStl.DetailsVisile = true;
            }
            WorkTag = new TagWrap("текущий");
            WorkTag.Tag.ViewStl.DetailsVisile = true;
        }
        [DataMember]
        private TagWrap[][] CellsXml
        {
            get
            {
                var tmp = new TagWrap[Cells.GetLength(0)][];
                for (int r = 0; r < Cells.GetLength(0); r++)
                {
                    tmp[r] = new TagWrap[Cells.GetLength(1)];
                    for (int c = 0; c < Cells.GetLength(1); c++)
                        tmp[r][c] = Cells[r, c];
                }
                return tmp;
            }
            set
            {
                if (!value.Any())
                {
                     Cells = new TagWrap[0,0];
                    return;
                }
                Cells = new TagWrap[value.Length, value[0].Length];
                for (int r = 0; r < value.GetLength(0); r++)
                    for (int c = 0; c < value[0].Length; c++)
                        Cells[r, c] = value[r][c];
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    [DataContract]
    public class TagWrap: INotifyPropertyChanged
    {
        [DataMember] private Tag _tag;

        public TagWrap(string header = "")
        {
            _tag = new Tag();
            _tag.Header = header;
        }
        public TagWrap(Tag tag)
        {
            _tag = tag;
        }
        public Tag Tag
        {
            get { return _tag; }
            set
            {
                if (Equals(value, _tag)) return;
                _tag = value;
                OnPropertyChanged();
            }
        }

        public void SwapWith(TagWrap second, DragProcessor.SwapMode mode)
        {
            if (second == null) second = new TagWrap();
            if (object.ReferenceEquals(this, second)) return;
            switch (mode)
            {
                case DragProcessor.SwapMode.Copy:
                    Tag = second.Tag.GetClone(this); break;
                case DragProcessor.SwapMode.Swap:
                    var tmpTag = second.Tag;
                    second.Tag = Tag;
                    Tag = tmpTag; break;
                default:
                    Tag = second.Tag;
                    second.Tag = new Tag(); break;
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
