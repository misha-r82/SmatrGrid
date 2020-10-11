using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartGrid;
using SmartGrid.Items;
using Test;

namespace UnitTestProject
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            //Node node = new Node("123");
            //var serialized = FileIO.SerializeJson(node);
            Tag1 tag = new Tag1("654");
            var serialized = FileIO.SerializeJson(tag);
            var field = new SmartFiled("23456");
            serialized = FileIO.SerializeJson(field);
        }
    }
    [DataContract]
    public class Tag1 : HeaderableList<Node>, ICloneableEx<SmartGrid.Tag>
    {
        [DataMember] public string str = "123";
        [DataMember] public ViewStyle ViewStl { get; set; }

        public Tag1(string header = "") : base(header)
        {
            ViewStl = new ViewStyle();
        }
        public bool IsEmptyTag
        {
            get => !this.Any() && string.IsNullOrEmpty(Header.Header);
        }

        public object Clone()
        {
            return GetClone();
        }
        public void CloneRefs()
        {
            var tmp = this.ToList();
            Clear();
            foreach (Node node in tmp)
                Add(node);
            Header.CloneRefs();
        }
        public SmartGrid.Tag GetClone()
        {
            var clone = (SmartGrid.Tag)MemberwiseClone();
            clone.CloneRefs();
            return clone;
        }
    }
}
