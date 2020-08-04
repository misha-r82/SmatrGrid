using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartGrid.Drag;
using SmartGrid;
namespace UnitTestProject
{
    [TestClass]
    public class DragElementCreateTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var element1 = new FrameworkElement();
            element1.DataContext = new Tag("1");
            var element2 = new FrameworkElement();
            element1.DataContext = new SmartFiled();
            //var evArgs= new DragEventArgs(element2, new Point(0,0) );

        }
    }
}
