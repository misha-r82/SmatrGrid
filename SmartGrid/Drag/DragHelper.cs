using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SmartGrid.Drag
{
    class DragHelper
    {
        private static Point _pt;
        private static DragProcessor.DragData<IHasHeader, IHasHeader> _data;

        private static bool CopyMode { get { return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl); } }
        private static DragDropEffects DragEffect
        {
            get { return CopyMode ? DragDropEffects.Copy : DragDropEffects.Move; }
        }
        public static void SetClick(DragProcessor.DragContent data, MouseButtonEventArgs e)
        {
            _pt = e.GetPosition(null);
            _data = data;
        }
        public static void Track(MouseEventArgs e)
        {
            if (_data == null) return;
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _data = null;
                return;
            }
            var diff = e.GetPosition(null) - _pt;
            if (Math.Abs(diff.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) < SystemParameters.MinimumVerticalDragDistance) return;
            DragDrop.DoDragDrop((FrameworkElement)e.Source, _data, DragEffect);
        }
    }
}
