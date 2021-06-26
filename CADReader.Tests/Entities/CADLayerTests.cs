using CAD.Entity;
using CAD.Geometry;
using CAD.Nomenclature;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using RBush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Tests.Entities
{
    [TestClass]
    public class CADLayerTests
    {
        private static CADLayer layer;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            layer = new CADLayer(CADLayerType.CADASTER.ToString());
        }

        [TestMethod]
        public void T1_CreatesNewCADLayer()
        {
            Assert.IsNotNull(layer);
            Assert.AreEqual(CADLayerType.CADASTER.ToString(), layer.Name);
            Assert.AreEqual(CADLayerType.CADASTER, layer.Type);
            Assert.IsNotNull(layer.Entities);
            Assert.IsInstanceOfType(layer.Entities, typeof(List<ICADEntity>));
            Assert.AreEqual(0, layer.EntitiesCount);
        }

        [TestMethod]
        public void T2_AddsEntitiesToCADLayer()
        {
            List<string[]> points = new List<string[]>(){
                new string[17]{"28", "2158", "632.640", "1537.090", "0.000", "0", "0.00", "0.00", "0", "0.00", "0", "0", "0", "0", "\"\"", "25.11.2008", "0"},
                new string[17]{"28", "2159", "634.870", "1559.220", "0.000", "0", "0.00", "0.00", "0", "0.00", "0", "0", "0", "0", "\"\"", "25.11.2008", "0"},
                new string[17]{"28", "2160", "626.270", "1549.520", "0.000", "0", "0.00", "0.00", "0", "0.00", "0", "0", "0", "0", "\"\"", "25.11.2008", "0"},
                new string[17]{"28", "2161", "626.940", "1553.410", "0.000", "0", "0.00", "0.00", "0", "0.00", "0", "0", "0", "0", "\"\"", "25.11.2008", "0"},
                new string[17]{"28", "2162", "621.210", "1554.070", "0.000", "0", "0.00", "0.00", "0", "0.00", "0", "0", "0", "0", "\"\"", "25.11.2008", "0"}
            };

            foreach (string[] values in points)
                layer.AddEntity(new CADPoint(values));

            Assert.AreEqual(points.Count, layer.EntitiesCount);
        }

        [TestMethod]
        public void T3_SearchEntityByGUID()
        {
            string guid = layer.Entities[0].GUID;

            ICADEntity result = layer.Search(i => i.GUID.Equals(guid)).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(layer.Entities[0], result);
        }

        [TestMethod]
        public void T4_SearchEntityByProperties()
        {
            int number = 2160;

            ICADEntity result = layer.Search(i =>
            {
                if (i is CADPoint point && point.Number == number)
                    return true;
                return false;
            }).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(number, (result as CADPoint).Number);
        }

        [TestMethod]
        public void T5_SearchEntitiesByType()
        {
            List<ICADEntity> pointEntities = layer.Search(i => i is CADPoint);

            Assert.IsNotNull(pointEntities);
            Assert.IsInstanceOfType(pointEntities, typeof(List<ICADEntity>));
            Assert.AreEqual(layer.EntitiesCount, pointEntities.Count);
        }

        [TestMethod]
        public void T6_SearchEntitiesInExtent()
        {
            /**
             * Tests are based on the points in T1_AddsEntitiesToCADLayer():
             */

            /**
             *  Extent: 631.362, 1553.444, 638.741, 1561.818
             * Results: 2159
             */
            Extent envelope = new Extent(631.362, 1553.444, 638.741, 1561.818);
            List<ICADEntity> pointEntities = layer.Search(envelope);

            Assert.IsNotNull(pointEntities);
            Assert.IsInstanceOfType(pointEntities, typeof(List<ICADEntity>));
            Assert.AreEqual(1, pointEntities.Count);
            Assert.AreEqual(2159, (pointEntities[0] as CADPoint).Number);

            /**
             *  Extent: 619.358, 1544.629, 628.829, 1558.072
             * Results: 2160, 2161, 2162
             */
            envelope = new Extent(619.358, 1544.629, 628.829, 1558.072);
            pointEntities = layer.Search(envelope);

            Assert.IsNotNull(pointEntities);
            Assert.IsInstanceOfType(pointEntities, typeof(List<ICADEntity>));
            Assert.AreEqual(3, pointEntities.Count);
            Assert.AreEqual(2160, (pointEntities[0] as CADPoint).Number);
            Assert.AreEqual(2161, (pointEntities[1] as CADPoint).Number);
            Assert.AreEqual(2162, (pointEntities[2] as CADPoint).Number);

            /**
             *  Extent: 616.936, 1534.712, 640.833, 1563.691
             * Results: 2158, 2159, 2160, 2161, 2162
             */
            envelope = new Extent(616.936, 1534.712, 640.833, 1563.691);
            pointEntities = layer.Search(envelope);

            Assert.IsNotNull(pointEntities);
            Assert.IsInstanceOfType(pointEntities, typeof(List<ICADEntity>));
            Assert.AreEqual(5, pointEntities.Count);
            Assert.AreEqual(2158, (pointEntities[0] as CADPoint).Number);
            Assert.AreEqual(2159, (pointEntities[1] as CADPoint).Number);
            Assert.AreEqual(2160, (pointEntities[2] as CADPoint).Number);
            Assert.AreEqual(2161, (pointEntities[3] as CADPoint).Number);
            Assert.AreEqual(2162, (pointEntities[4] as CADPoint).Number);

            /**
             *  Extent: 633.344, 1541.874, 638.741, 1547.714 
             * Results: 0
             */
            envelope = new Extent(633.344, 1541.874, 638.741, 1547.714);
            pointEntities = layer.Search(envelope);

            Assert.IsNotNull(pointEntities);
            Assert.IsInstanceOfType(pointEntities, typeof(List<ICADEntity>));
            Assert.AreEqual(0, pointEntities.Count);
        }

        [TestMethod]
        public void T7_RemovesEntitiesFromCADLayer()
        {
            int count = layer.EntitiesCount;
            ICADEntity entity = layer.Entities[0];

            layer.RemoveEntity(entity);

            Assert.AreEqual(count - 1, layer.EntitiesCount);
        }
    }
}
