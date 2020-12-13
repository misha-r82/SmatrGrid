using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGrid.Grid
{
    public class WidthManager
    {
        private double EPS = 0.1;
        private GridWidth _gw;
        public WidthManager(GridWidth gridWidth)
        {
            _gw = gridWidth;
        }

        private double H12 => _gw.H1 + _gw.H2;
        private double H13 => _gw.H1 + _gw.H3;
        private double H23 => _gw.H2 + _gw.H3;
        private double W12 => _gw.W1 + _gw.W2;
        private double W13 => _gw.W1 + _gw.W3;
        private double W23 => _gw.W2 + _gw.W3;
        private double Wall => _gw.W1 + _gw.W2 + _gw.W3;
        private double Hall => _gw.H1 + _gw.H2 + _gw.H3;
        private int Rows
        {
            get
            {
                int rows = 0;
                if (_gw.H1 > EPS) rows++;
                if (_gw.H2 > EPS) rows++;
                if (_gw.H3 > EPS) rows++;
                return rows;
            }
        }
        private int Cols
        {
            get
            {
                int cols = 0;
                if (_gw.W1 > EPS) cols++;
                if (_gw.W2 > EPS) cols++;
                if (_gw.W3 > EPS) cols++;
                return cols;
            }
        }
        public void MiddleH12()
        {
            _gw.H1 = _gw.H2 = H12 / 2;
        }
        public void MiddleH23()
        {
            _gw.H2 = _gw.H3 = H23 / 2;
        }
        public void MiddleW12()
        {
            _gw.W1 = _gw.W2 = W12 / 2;
        }
        public void MiddleW23()
        {
            _gw.W2 = _gw.W3 = W23 / 2;
        }

        public void ShowAll()
        {
            _gw.W1 = _gw.W2 = _gw.W3 = _gw.H1 = _gw.H2 = _gw.H3 = 100;
        }

        public void ShowHideW1()
        {
            if (W23 < EPS) return;
            if (_gw.W1 < EPS) _gw.W1 = Wall / Cols;
            else _gw.W1 = 0;
        }
        public void ShowHideW2()
        {
            if (W13 < EPS) return;
            if (_gw.W2 < EPS) _gw.W2 = Wall / Cols;
            else _gw.W2 = 0;
        }
        public void ShowHideW3()
        {
             if (W12 < EPS) return;
            if (_gw.W3 < EPS) _gw.W3 = Wall / Cols;
            else _gw.W3 = 0;
        }
        public void ShowHideH1()
        {
            if (H23 < EPS) return;
            if (_gw.H1 < EPS) _gw.H1 = Hall / Rows;
            else _gw.H1 = 0;
        }
        public void ShowHideH2()
        {
            if(H13 < EPS) return;
            if (_gw.H2 < EPS) _gw.H2 = Hall / Rows;
            else _gw.H2 = 0;
        }
        public void ShowHideH3()
        {
            if (H12 < EPS) return;
            if (_gw.H3 < EPS) _gw.H3 = Hall / Rows;
            else _gw.H3 = 0;
        }
        public void HorAlign()
        {
            double width = Wall / Rows;
            if (_gw.W1 > EPS) _gw.W1 = width;
            if (_gw.W2 > EPS) _gw.W2 = width;
            if (_gw.W3 > EPS) _gw.W3 = width;
        }
        public void VertAlign()
        {
            double height = Hall / Cols;
            if (_gw.H1 > EPS) _gw.H1 = height;
            if (_gw.H2 > EPS) _gw.H2 = height;
            if (_gw.H3 > EPS) _gw.H3 = height;

        }
    }
}
