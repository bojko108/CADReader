using CAD.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAD.Tests.Geometry
{
    [TestClass]
    public class CADReaderTests
    {
        [TestMethod]
        public void AddsPoints()
        {
            Point reference = new Point(10, 10);
            reference.GUID = "reference";
            Point relative = new Point(5, 5);
            relative.GUID = "relative";

            Point absolute = reference + relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(15, absolute.N);
            Assert.AreEqual(15, absolute.E);

            relative = new Point(-5, 5);
            absolute = reference + relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(5, absolute.N);
            Assert.AreEqual(15, absolute.E);

            relative = new Point(-5, -5);
            absolute = reference + relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(5, absolute.N);
            Assert.AreEqual(5, absolute.E);
        }

        [TestMethod]
        public void SubstractsPoints()
        {
            Point reference = new Point(10, 10);
            reference.GUID = "reference";
            Point relative = new Point(5, 5);
            relative.GUID = "relative";

            Point absolute = reference - relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(5, absolute.N);
            Assert.AreEqual(5, absolute.E);

            relative = new Point(-5, 5);
            absolute = reference - relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(15, absolute.N);
            Assert.AreEqual(5, absolute.E);

            relative = new Point(-5, -5);
            absolute = reference - relative;

            Assert.IsNotNull(absolute);
            Assert.AreEqual(relative.GUID, absolute.GUID);
            Assert.AreEqual(15, absolute.N);
            Assert.AreEqual(15, absolute.E);
        }

        [TestMethod]
        public void CheckPointsForEquality()
        {
            Point a = new Point(10, 10);
            a.GUID = "a";
            Point b = new Point(5, 5);
            b.GUID = "b";
            Point c = new Point(5, 5);
            c.GUID = "c";

            Assert.IsTrue(a != b);
            Assert.IsTrue(a != c);
            Assert.IsFalse(b != c);
            Assert.IsTrue(b == c);
        }
    }
}
