﻿using System.Collections.Generic;
using System.IO;
using ByContext.Model;
using NUnit.Framework;

namespace ByContext.XML.Tests
{
    [TestFixture]
    public class ExportXmlTest
    {
        [Test]         
        public void ExportXml()
        {
            string xml = File.ReadAllText("Negate.xml");
            IEnumerable<Section> sections = new XmlLoader().ReadXml(xml);
            string exportedXml = new XmlLoader().ToXml(sections);

            Assert.AreEqual(xml.Replace(" ", string.Empty), exportedXml.Replace(" ", string.Empty));
        }
    }
}