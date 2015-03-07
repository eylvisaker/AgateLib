using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Geometry
{
    [TestClass]
    public class RectangleTest
    {
        [TestMethod]
        public void FromStringDelimited()
        {
            var rect = Rectangle.FromString("2, 3, 4, 5");

            Assert.AreEqual(2, rect.X);
            Assert.AreEqual(3, rect.Y);
            Assert.AreEqual(4, rect.Width);
            Assert.AreEqual(5, rect.Height);
        }

        [TestMethod]
        public void FromStringNamedArgs()
        {
            var rect = Rectangle.FromString("x = 2, y = 3, width = 4, height = 5");

            Assert.AreEqual(2, rect.X);
            Assert.AreEqual(3, rect.Y);
            Assert.AreEqual(4, rect.Width);
            Assert.AreEqual(5, rect.Height);
        }
    }
}
