using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Extensions;
using sys = System.ComponentModel;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PropertyInfoExtensionsTest
    {
        public string PropertyWithNoAttributes { get; set; }

        [sys.Description("Dummy property")]
        public string PropertyWithOneAttribute { get; set; }

        [TestMethod]
        public void GetFirstAttribute_NoAttributes()
        {
            var property = typeof(PropertyInfoExtensionsTest).GetAccessiblePublicInstanceProperties().Single(x => x.Name == "PropertyWithNoAttributes");
            var description = property.GetFirstAttribute<sys.DescriptionAttribute>();
            Assert.IsNull(description);
        }

        [TestMethod]
        public void GetFirstAttribute_OneAttribute()
        {
            var property = typeof(PropertyInfoExtensionsTest).GetAccessiblePublicInstanceProperties().Single(x => x.Name == "PropertyWithOneAttribute");
            var description = property.GetFirstAttribute<sys.DescriptionAttribute>();
            Assert.IsNotNull(description);
            Assert.AreEqual("Dummy property", description.Description);
        }
    }
}
