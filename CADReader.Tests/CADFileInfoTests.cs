using CAD.Geometry;
using CAD.Nomenclature;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAD.Tests
{
    [TestClass]
    public class CADFileInfoTests
    {
        private static CADFileInfo info;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            info = new CADFileInfo();
            Assert.IsNotNull(info);
        }

        [TestMethod]
        public void SetsCADVersion()
        {
            info.SetVersion("4");
            Assert.AreEqual(CADVersion.Unknown, info.Version);

            info.SetVersion("4.00");
            Assert.AreEqual(CADVersion.v400, info.Version);

            info.SetVersion("4.03");
            Assert.AreEqual(CADVersion.v403, info.Version);
        }

        [TestMethod]
        public void SetsCoordinateSystem()
        {
            string bgs193027 = "1930, Балтийска, 9";
            info.SetCoordtype(bgs193027);
            Assert.AreEqual(bgs193027, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.BGS_1930_27, info.CoordinateSystem);

            string bgs1950621 = "1950, Балтийска, 4";
            info.SetCoordtype(bgs1950621);
            Assert.AreEqual(bgs1950621, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.BGS_1950_6_21, info.CoordinateSystem);

            string bgs1970k9 = "1970, Балтийска, K9";
            info.SetCoordtype(bgs1970k9);
            Assert.AreEqual(bgs1970k9, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.BGS_1970_K9, info.CoordinateSystem);

            string bgs2005utm35n = "2005 UTM, Балтийска, 35";
            info.SetCoordtype(bgs2005utm35n);
            Assert.AreEqual(bgs2005utm35n, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.UTM35N, info.CoordinateSystem);

            string bgs2005kk = "2005, Балтийска, 35";
            info.SetCoordtype(bgs2005kk);
            Assert.AreEqual(bgs2005kk, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.BGS_2005_KK, info.CoordinateSystem);

            string unknown = "20a05, Балтийска, 35";
            info.SetCoordtype(unknown);
            Assert.AreEqual(unknown, info.Coordtype);
            Assert.AreEqual(CoordinateSystem.Unknown, info.CoordinateSystem);
        }
    }
}
