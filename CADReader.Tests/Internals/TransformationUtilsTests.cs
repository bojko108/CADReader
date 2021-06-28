using CAD.Geometry;
using CAD.Internals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAD.Tests.Internals
{
    [TestClass]
    public class TransformationUtilsTests
    {
        [TestMethod]
        public void Transforms1970k9ToBGS2005KK()
        {
            Assert.IsNotNull(TransformationUtils.Instance);

            TransformationUtils.Instance.SourceProjection = CoordinateSystem.BGS_1970_K9;
            TransformationUtils.Instance.TargetProjection = CoordinateSystem.BGS_2005_KK;

            Point k9 = new Point(4547844.976, 8508858.179);

            TransformationUtils.Instance.CalculateTransformationParameters(Extent.FromEnvelope(k9.Envelope));

            Point bgs2005kk = TransformationUtils.Instance.TransformPoint(k9);

            double N = 4675440.845,
                E = 330568.432;

            Assert.AreEqual(N, bgs2005kk.N, 0.1);
            Assert.AreEqual(E, bgs2005kk.E, 0.1);
        }

        [TestMethod]
        public void Transforms1970k9ToWGS84()
        {
            Assert.IsNotNull(TransformationUtils.Instance);

            TransformationUtils.Instance.SourceProjection = CoordinateSystem.BGS_1970_K9;
            TransformationUtils.Instance.TargetProjection = CoordinateSystem.WGS84_GEOGRAPHIC;

            Point k9 = new Point(4547844.976, 8508858.179);

            TransformationUtils.Instance.CalculateTransformationParameters(Extent.FromEnvelope(k9.Envelope));

            Point wgs84 = TransformationUtils.Instance.TransformPoint(k9);

            double lat = 42.195768,
                lon = 23.448409;

            Assert.AreEqual(lat, wgs84.N, 0.00001);
            Assert.AreEqual(lon, wgs84.E, 0.00001);
        }
    }
}
