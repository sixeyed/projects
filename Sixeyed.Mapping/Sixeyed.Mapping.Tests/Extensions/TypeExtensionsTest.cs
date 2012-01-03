using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TypeExtensionsTest
    {
        public string PublicProperty { get; set; }

        public string PublicPropertyWithPrivateGet { private get; set; }

        private string PrivateProperty { get; set; }

        [TestMethod]
        public void GetAccessiblePublicInstanceProperties()
        {
            var properties = typeof(TypeExtensionsTest).GetAccessiblePublicInstanceProperties();
            Assert.AreEqual(1, properties.Count());
            Assert.AreEqual("PublicProperty", properties.ElementAt(0).Name);
        }
    }
}
