using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Lib.Controls.ColorPiecker
{
    /// <summary>
    /// Логика взаимодействия для CtrlColorPiecker.xaml
    /// </summary>
    public partial class CtrlColorPiecker : UserControl
    {
        public CtrlColorPiecker()
        {
            InitializeComponent();
            DataContext = this;
            InitialWork();

        }

        private Color _customColor = Colors.Transparent;

        public Color CustomColor
        {
            get { return _customColor; }
            set
            {
                if (_customColor != value)
                {
                    _customColor = value;
                }
            }
        }

        private void InitialWork()
        {
            DefaultPicker.Items.Clear();
            CustomColors customColors = new CustomColors(
                new CustomColors.Range(0,360,27),
                new CustomColors.Range(1,0.5,2),
                new CustomColors.Range(0.6,0.9,3)
                
                );
            foreach (var item in customColors.SelectableColors)
            {
                DefaultPicker.Items.Add(item);
            }
        }
        }

        class CustomColors
        {
            List<Color> _SelectableColors = null;

            public List<Color> SelectableColors
            {
                get { return _SelectableColors; }
                set { _SelectableColors = value; }
            }


        public class Range
        {
            public Range(double start, double end, int count)
            {
                if (count < 0) throw new ArgumentException("Число интервалов CustomColors.Range должно быть > 0!");
                this.start = start;
                this.end = end;
                this.count = count;
            }
            public double start;
            public double end;
            public int count;
            public double Step => count == 1 ? end - start : (end - start) / (count-1);
            private bool Iterated(double cur)
            {
                if (start < end) return cur <= end;
                return cur >= end;
            }
            public IEnumerable<double> Values
            {
                get
                {
                    if (count == 0) yield break;
                    for (double cur = start; Iterated(cur); cur += Step)
                        yield return cur;
                }
            }
        }
        public CustomColors(Range hue, Range sat, Range light)
        {
            var tmp1 = sat.Values.ToArray();
            var tmp = light.Values.ToArray();
            _SelectableColors = new List<Color>();
            foreach (var h in hue.Values)
            foreach (var s in sat.Values)
            foreach (var l in light.Values)
            {
                _SelectableColors.Add(UI.ColorUtils.HSLToRGB(h, s, l));
            }

            }

        }


}
