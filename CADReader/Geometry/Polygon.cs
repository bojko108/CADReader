using RBush;
using System;
using System.Collections.Generic;

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
        public List<Point> Vertices { get; private set; }
        /// <summary>
        /// Дължина на полигона (обиколка)
        /// </summary>
        public double Length => throw new NotImplementedException();
        /// <summary>
        /// Площ на полигона
        /// </summary>
        public double Area => throw new NotImplementedException();

        /// <summary>
        /// Обхват на геометрията - служи при търсене в пространственият индекс
        /// </summary>
        public ref readonly Envelope Envelope => ref _envelope;
        private readonly Envelope _envelope;

        /// <summary>
        /// Създава нов полигон
        /// </summary>
        /// <param name="vertices">точки, описващи границата на полигона</param>
        public Polygon(List<Point> vertices)
        {
            this.Vertices = vertices;

            // This should be tested:
            // Initializes the extent of this geometry. 
            // Once the extent is calculated it shouldn't change 
            // as it is part of the spatial index
            this._envelope = new Envelope(double.MaxValue, double.MaxValue, double.MinValue, double.MinValue);
            foreach (Point point in this.Vertices)
                this._envelope = this._envelope.Extend(point.Envelope);
        }
    }
}
