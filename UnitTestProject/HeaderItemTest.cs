using System;
using System.IO;
using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartGrid.HeaderIcons;

namespace UnitTestProject
{
    [TestClass]
    public class HeaderItemTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var stream = new FileStream(@"C:\Users\misha\RiderProjects\SmartGrid\smartgrid\SmartGrid\img\file.png", FileMode.Open);
            var item = new HeaderIcon(stream);
            FileIO.SerializeDataContract(item, @"C:\Users\misha\RiderProjects\SmartGrid\smartgrid\SmartGrid\img\file1.xml");
            var deserialized =
                FileIO.DeserializeDataContract<HeaderIcon>(@"C:\Users\misha\RiderProjects\SmartGrid\smartgrid\SmartGrid\img\file1.xml");

        }
    }
}
