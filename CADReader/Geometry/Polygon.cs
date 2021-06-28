using RBush;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAD.Geometry
{
    /// <summary>
    /// Представлява полигон
    /// </summary>
    public class Polygon : IGeometry, IGeometryInfo
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// Списък с точки, описващи границата на полигона
        /// </summary>
        public List<Point> Vertices { get; private set; } = new List<Point>();
        /// <summary>
        /// Списък с линии, описващи границата на полигона
        /// </summary>
        public List<Polyline> Polylines { get; private set; } = new List<Polyline>();
        /// <summary>
        /// Дължина на полигона (обиколка)
        /// </summary>
        public double Length => throw new NotImplementedException();
        /// <summary>
        /// Площ на полигона
        /// </summary>
        public double Area => throw new NotImplementedException();

        /// <summary>
        /// Координатна система
        /// </summary>
        public CoordinateSystem CoordinateSystem { get; set; }

        /// <summary>
        /// Обхват на геометрията - служи при търсене в пространственият индекс
        /// </summary>
        public ref readonly Envelope Envelope => ref _envelope;
        private readonly Envelope _envelope;

        /// <summary>
        /// Създава нов полигон
        /// </summary>
        /// <param name="polylines">линии, описващи границата на полигона</param>
        public Polygon(List<Polyline> polylines)
        {
            this.Polylines = polylines;

            // This should be tested:
            // Initializes the extent of this geometry. 
            // Once the extent is calculated it shouldn't change 
            // as it is part of the spatial index
            this._envelope = new Envelope(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);
            foreach (Polyline polyline in this.Polylines)
                this._envelope = this._envelope.Extend(polyline.Envelope);
        }

        /// <summary>
        /// Създава копие на този полигон
        /// </summary>
        /// <returns></returns>
        public IGeometry CopyGeometry()
        {
            List<Polyline> newPolylines = new List<Polyline>();
            foreach (Polyline polyline in this.Polylines)
                newPolylines.Add((Polyline)polyline.CopyGeometry());
            return new Polygon(newPolylines);
        }
    }
}
