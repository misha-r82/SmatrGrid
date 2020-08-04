using System.Windows.Forms;

namespace SmartGrid.Undo
{
 /*   public class TagUndoScope : UndoScope
    {
        private TagWrap _tagWrap;
        private Tag _oldValue;
        public override string Name
        {
            get { return _tagWrap.Tag.IsEmptyTag ? $"Очистка набора" : $"Изменение набора {_oldValue.Header}"; }
        }

        public TagUndoScope(TagWrap tagWrap)
        {
            _tagWrap = tagWrap;
            _oldValue = _tagWrap.Tag;
        }

        private void Swap()
        {
            Tag tmp = _oldValue;
            _oldValue = _tagWrap.Tag;
            _tagWrap.Tag = tmp;
        }
        public override void Undo()
        {
            Swap();
        }

        public override void Rendo()
        {
            Swap();
        }

        public override bool HasChanges => _oldValue.Equals(_tagWrap.Tag);
    }*/
}