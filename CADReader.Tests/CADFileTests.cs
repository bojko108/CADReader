using CAD.Entity;
using CAD.Geometry;
using CAD.Internals;
using CAD.Nomenclature;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace CAD.Tests
{
    [TestClass]
    public class CADFileTests
    {
        private static CADFile reader;
        private const string filePath = "../../../files/test1-cad4.cad";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            if (context.TestName.Equals(nameof(ThrowsExceptionWhenVersionNot4))
                || context.TestName.Equals(nameof(ReadsCADFileFromMemory)))
            {

            }
            else
            {
                reader = new CADFile(filePath);
                Assert.IsNotNull(reader);
                reader.ReadFile();
            }
        }

        [TestMethod]
        public void ReadsCADFileFromMemory()
        {
            byte[] contents = File.ReadAllBytes(filePath);

            Assert.IsTrue(contents.Length > 0);

            CADFile test = new CADFile(contents);
            Assert.IsNotNull(test);
            test.ReadFile();

            CADFileInfo info = test.FileInfo;
            Assert.IsNotNull(info);
            Assert.AreEqual(CADVersion.v400, info.Version);
            Assert.AreEqual("37914", info.EKATTE);
            Assert.AreEqual("с.Кокаляне", info.Name);
            Assert.AreEqual("CadIS v4.27", info.Program);
            Assert.AreEqual("\"ГЕОМЕРА М+Р\" ЕООД", info.Firm);
            Assert.AreEqual(4589000.000, info.ReferencePoint.N);
            Assert.AreEqual(8505000.000, info.ReferencePoint.E);
            Assert.AreEqual(4589000.000 + 261.000, info.Window.MinN);
            Assert.AreEqual(8505000.000 + 645.000, info.Window.MinE);
            Assert.AreEqual(4589000.000 + 2198.000, info.Window.MaxN);
            Assert.AreEqual(8505000.000 + 2425.000, info.Window.MaxE);
            Assert.AreEqual("1970, Балтийска, K9", info.Coordtype);
            Assert.AreEqual(CADContentType.PART, info.Contents);
            Assert.AreEqual("", info.Comment);
        }

        [TestMethod]
        public void ChangesCADFileProjection()
        {
            CADFile cad = new CADFile(filePath, CoordinateSystem.WGS84_GEOGRAPHIC);
            Assert.IsNotNull(cad);
            cad.ReadFile();
            Assert.IsNotNull(cad.FileInfo);

            Point referencePoint = cad.FileInfo.ReferencePoint;
            Assert.AreEqual(CoordinateSystem.WGS84_GEOGRAPHIC, referencePoint.CoordinateSystem);

            CADPoint point = cad[CADLayerType.CADASTER].Entities[0] as CADPoint;
            Assert.AreEqual(CoordinateSystem.WGS84_GEOGRAPHIC, point.Geometry.CoordinateSystem);
        }

        [TestMethod]
        public void ReadsCADHeaderSection()
        {
            /**
             * VERSION 4.00
             * EKATTE 37914
             * NAME с.Кокаляне
             * PROGRAM CadIS v4.27
             * DATE 09.03.2009
             * FIRM "ГЕОМЕРА М+Р" ЕООД
             * REFERENCE 4589000.000 8505000.000
             * WINDOW 261.000 645.000 2198.000 2425.000
             * COORDTYPE 1970, Балтийска, K9
             * CONTENTS PART
             * COMMENT 
             */

            CADFileInfo info = reader.FileInfo;

            Assert.IsNotNull(info);
            Assert.AreEqual(CADVersion.v400, info.Version);
            Assert.AreEqual("37914", info.EKATTE);
            Assert.AreEqual("с.Кокаляне", info.Name);
            Assert.AreEqual("CadIS v4.27", info.Program);
            Assert.AreEqual("\"ГЕОМЕРА М+Р\" ЕООД", info.Firm);
            Assert.AreEqual(4589000.000, info.ReferencePoint.N);
            Assert.AreEqual(8505000.000, info.ReferencePoint.E);
            Assert.AreEqual(4589000.000 + 261.000, info.Window.MinN);
            Assert.AreEqual(8505000.000 + 645.000, info.Window.MinE);
            Assert.AreEqual(4589000.000 + 2198.000, info.Window.MaxN);
            Assert.AreEqual(8505000.000 + 2425.000, info.Window.MaxE);
            Assert.AreEqual("1970, Балтийска, K9", info.Coordtype);
            Assert.AreEqual(CADContentType.PART, info.Contents);
            Assert.AreEqual("", info.Comment);
        }

        [TestMethod]
        public void ReadsCADGraphicsSection()
        {
            Assert.IsNotNull(reader.Layers);
            Assert.AreEqual(5, reader.Layers.Count);

            /**
             * CONTROL CADASTER
             * NUMBER_POINTS 3564
             * NUMBER_LINES 3963
             * NUMBER_SYMBOLS 0
             * NUMBER_TEXTS 471
             * NUMBER_CONTURS 1359
             */
            CADLayer cadastre = reader[CADLayerType.CADASTER];

            Assert.IsNotNull(cadastre);
            Assert.AreEqual(CADLayerType.CADASTER, cadastre.Type);
            Assert.AreEqual(CADLayerType.CADASTER.ToString(), cadastre.Name);

            int cp = cadastre.Entities.OfType<CADPoint>().Count();
            int cl = cadastre.Entities.OfType<CADLine>().Count();
            int cs = cadastre.Entities.OfType<CADSymbol>().Count();
            int ct = cadastre.Entities.OfType<CADText>().Count();
            int cc = cadastre.Entities.OfType<CADContour>().Count();

            Assert.AreEqual(3564, cp);
            Assert.AreEqual(3963, cl);
            Assert.AreEqual(0, cs);
            Assert.AreEqual(471, ct);
            Assert.AreEqual(1359, cc);

            /**
             * CONTROL LESO
             * NUMBER_POINTS 0
             * NUMBER_LINES 0
             * NUMBER_SYMBOLS 0
             * NUMBER_TEXTS 0
             * NUMBER_CONTURS 0
             * END_CONTROL
             */
            CADLayer leso = reader[CADLayerType.LESO];

            Assert.IsNotNull(leso);
            Assert.AreEqual(CADLayerType.LESO, leso.Type);
            Assert.AreEqual(CADLayerType.LESO.ToString(), leso.Name);

            cp = leso.Entities.OfType<CADPoint>().Count();
            cl = leso.Entities.OfType<CADLine>().Count();
            cs = leso.Entities.OfType<CADSymbol>().Count();
            ct = leso.Entities.OfType<CADText>().Count();
            cc = leso.Entities.OfType<CADContour>().Count();

            Assert.AreEqual(0, cp);
            Assert.AreEqual(0, cl);
            Assert.AreEqual(0, cs);
            Assert.AreEqual(0, ct);
            Assert.AreEqual(0, cc);

            /**
             * CONTROL REGPLAN
             * NUMBER_POINTS 0
             * NUMBER_LINES 0
             * NUMBER_SYMBOLS 0
             * NUMBER_TEXTS 0
             * NUMBER_CONTURS 0
             * END_CONTROL
             */
            CADLayer regplan = reader[CADLayerType.REGPLAN];

            Assert.IsNotNull(regplan);
            Assert.AreEqual(CADLayerType.REGPLAN, regplan.Type);
            Assert.AreEqual(CADLayerType.REGPLAN.ToString(), regplan.Name);

            cp = regplan.Entities.OfType<CADPoint>().Count();
            cl = regplan.Entities.OfType<CADLine>().Count();
            cs = regplan.Entities.OfType<CADSymbol>().Count();
            ct = regplan.Entities.OfType<CADText>().Count();
            cc = regplan.Entities.OfType<CADContour>().Count();

            Assert.AreEqual(0, cp);
            Assert.AreEqual(0, cl);
            Assert.AreEqual(0, cs);
            Assert.AreEqual(0, ct);
            Assert.AreEqual(0, cc);

            /**
             * CONTROL POCHKATEG
             * NUMBER_POINTS 0
             * NUMBER_LINES 0
             * NUMBER_SYMBOLS 0
             * NUMBER_TEXTS 0
             * NUMBER_CONTURS 0
             * END_CONTROL
             */
            CADLayer pochkateg = reader[CADLayerType.POCHKATEG];

            Assert.IsNotNull(pochkateg);
            Assert.AreEqual(CADLayerType.POCHKATEG, pochkateg.Type);
            Assert.AreEqual(CADLayerType.POCHKATEG.ToString(), pochkateg.Name);

            cp = pochkateg.Entities.OfType<CADPoint>().Count();
            cl = pochkateg.Entities.OfType<CADLine>().Count();
            cs = pochkateg.Entities.OfType<CADSymbol>().Count();
            ct = pochkateg.Entities.OfType<CADText>().Count();
            cc = pochkateg.Entities.OfType<CADContour>().Count();

            Assert.AreEqual(0, cp);
            Assert.AreEqual(0, cl);
            Assert.AreEqual(0, cs);
            Assert.AreEqual(0, ct);
            Assert.AreEqual(0, cc);

            /**
             * CONTROL SHEMI
             * NUMBER_POINTS 0
             * NUMBER_LINES 0
             * NUMBER_SYMBOLS 0
             * NUMBER_TEXTS 0
             * NUMBER_CONTURS 0
             * END_CONTROL
             */
            CADLayer shemi = reader[CADLayerType.SHEMI];

            Assert.IsNotNull(shemi);
            Assert.AreEqual(CADLayerType.SHEMI, shemi.Type);
            Assert.AreEqual(CADLayerType.SHEMI.ToString(), shemi.Name);

            cp = shemi.Entities.OfType<CADPoint>().Count();
            cl = shemi.Entities.OfType<CADLine>().Count();
            cs = shemi.Entities.OfType<CADSymbol>().Count();
            ct = shemi.Entities.OfType<CADText>().Count();
            cc = shemi.Entities.OfType<CADContour>().Count();

            Assert.AreEqual(0, cp);
            Assert.AreEqual(0, cl);
            Assert.AreEqual(0, cs);
            Assert.AreEqual(0, ct);
            Assert.AreEqual(0, cc);
        }

        [TestMethod]
        public void ThrowsExceptionWhenVersionNot4()
        {
            byte[] contents = File.ReadAllBytes("../../../files/test2-cad2.cad");

            Assert.IsTrue(contents.Length > 0);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                CADFile test = new CADFile(contents);
                Assert.IsNotNull(test);
                test.ReadFile();
            }, "Only CAD files v4 are supported");
        }

        [TestMethod]
        public void GetCADLayerByType()
        {
            CADLayer cadastre = reader[CADLayerType.CADASTER];
            Assert.IsNotNull(cadastre);
        }
    }
}
