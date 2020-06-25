using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartGrid;
using SmartGrid.Items;

namespace UnitTestProject
{
    [TestClass]
    public class HeaderableListTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var eventList = new List<NotifyCollectionChangedEventArgs>();
            var list = new HeaderableList<Node>();
            list.CollectionChanged += (sender, args) => eventList.Add(args);
            var node = new Node("1");
            list.Add(node);
        }
    }
}
