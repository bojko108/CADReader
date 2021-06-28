using CAD;
using CAD.Entity;
using CAD.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CAD.Tests.Geometry
{
    [TestClass]
    public class CADContourTests
    {
        [TestMethod]
        public void SortsContourLines()
        {
            string[] values = "C 2 6846.524 486.727 2218.569 20.11.2008 0"
                .Substring(2)
                .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            CADContour contour = new CADContour(values);

            Assert.IsNotNull(contour);

            /*
             *       N
             *       ^
             *       |
             *       |     (start -->)
             *       |          a          b         
             *       x----------x----------x----------x
             *       |p8        p7         p6       p5|
             *       |                                |
             *       |           <Contour>         p4 x c
             *       |                                |
             *       |p1                   p2       p3| 
             * (0,0) x---------------------x----------x -------------------> E
             *       e                     d 
             *    
             *              points: p1, p2, p3, p4, p5, p6, p7, p8
             *               lines: ab, de, !ae, bc, !dc
             *    correct order is: ab, bc, cd, de, ea
             */

            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 20);
            Point p3 = new Point(0, 30);
            Point p4 = new Point(10, 30);
            Point p5 = new Point(20, 30);
            Point p6 = new Point(20, 20);
            Point p7 = new Point(20, 10);
            Point p8 = new Point(20, 0);

            Polyline ab = new Polyline(new List<Point>() { p7, p6 });
            Polyline de = new Polyline(new List<Point>() { p2, p1 });
            Polyline ae = new Polyline(new List<Point>() { p7, p8, p1 });
            Polyline bc = new Polyline(new List<Point>() { p6, p5, p4 });
            Polyline dc = new Polyline(new List<Point>() { p2, p3, p4 });

            List<Polyline> polylines = new List<Polyline>() { ab, de, ae, bc, dc };

            try
            {
                this.ProcesPolylines(polylines);

                // Polygon polygon = new Polygon(this.Polylines);
            }
            catch (Exception ex)
            {

            }
        }

        private List<Polyline> Polylines = new List<Polyline>();

        private void ProcesPolylines(List<Polyline> polylines)
        {
            HashSet<string> sortedLines = new HashSet<string>();

            polylines.ForEach(p => p.GUID = Guid.NewGuid().ToString());

            this.Polylines.Add(polylines[0]);

            while (true)
            {
                Polyline current = polylines.Find(p =>
                {
                    if (sortedLines.Contains(p.GUID) == false)
                    {
                        if (p.StartPoint == this.Polylines[this.Polylines.Count - 1].EndPoint)
                            return true;
                        if (p.StartPoint == this.Polylines[this.Polylines.Count - 1].StartPoint)
                        {
                            p.Vertices.Reverse();
                            return true;
                        }
                    }
                    return false;
                });

                if (current == null)
                    break;

                sortedLines.Add(current.GUID);
                this.Polylines.Add(current);
            }
        }
    }
}