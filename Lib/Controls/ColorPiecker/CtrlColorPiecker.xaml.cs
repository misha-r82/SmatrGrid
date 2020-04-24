using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            InitialWork();

        }

        private bool _shift = false;
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
            CustomColors customColors = new CustomColors();
            foreach (var item in customColors.SelectableColors)
            {
                DefaultPicker.Items.Add(item);
            }
            DefaultPicker.SelectionChanged += new SelectionChangedEventHandler(DefaultPicker_SelectionChanged);
        }

        void DefaultPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DefaultPicker.SelectedValue != null)
            {
                _customColor = (Color)DefaultPicker.SelectedValue;
            }
            FrameworkElement frameworkElement = this;
            while (true)
            {
                if (frameworkElement == null) break;
                if (frameworkElement is ContextMenu)
                {
                    ((ContextMenu)frameworkElement).IsOpen = false;
                    break;
                }
                if (frameworkElement.Parent != null)
                    frameworkElement = (FrameworkElement)frameworkElement.Parent;
                else
                    break;
            }
        }

        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
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


        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        private Color HsvToRgb(double h, double S, double V)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            byte r = (byte)Clamp((int)(R * 255.0));
            byte g = (byte)Clamp((int)(G * 255.0));
            byte b = (byte)Clamp((int)(B * 255.0));
            return Color.FromRgb(r,g,b);
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }




        public CustomColors()
            {
                _SelectableColors = new List<Color>();
                byte step = 20;
                byte down = 0;
                int up = 360;
                for (double h = 0; h < up; h+=step)
                for (double l = 0.6; l < 1; l += 0.2)
                for (double s = 0.6; s < 1; s+=0.2)

                {

                    
                    _SelectableColors.Add(HsvToRgb(h,s,l));
                }

            }

        }


}
