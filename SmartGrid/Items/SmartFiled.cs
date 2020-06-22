using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SmartGrid.Annotations;

namespace SmartGrid
{
    [DataContract]
    public class SmartFiled : INotifyPropertyChanged
    {
        public TagWrap[,] Cells;
        [DataMember] public TagWrap WorkTag { get; set;}
        [DataMember] public TagGroup TagGrp { get; private set; }
        [DataMember] public GridWidth GridWidth { get; private set; }
        [DataMember] private string _header;
        private bool _isEditMode;
        public string Header
        {
            get { return _header; }
            set
            {
                if (_header == value) return;
                _header = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode == value) return;
                _isEditMode = value;
                OnPropertyChanged();
            }
        }
        public SmartFiled(string header = "")
        {
            Header = header;
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
}
