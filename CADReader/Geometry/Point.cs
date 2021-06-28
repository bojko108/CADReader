using BojkoSoft.Transformations;
using CAD.Entity;
using RBush;
using System;

namespace CAD.Geometry
{
    /// <summary>
    /// Представлява точка
    /// </summary>
    public class Point : IGeometry, IPoint, IEquatable<Point>
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// Northing в метри (x в CAD файл)
        /// </summary>
        public double N { get; set; }
        /// <summary>
        /// Easting в метри (y в CAD файл)
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// Z в метри (h в CAD файл)
        /// </summary>
        public double Z { get; set; }
        /// <summary>
        /// Информация за точка от линия
        /// </summary>
        public PointInfo PointInfo { get; set; }

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
        /// Създава нова точка на координати 0, 0, 0
        /// </summary>
        public Point()
            : this(0.0, 0.0, 0.0) { }

        /// <summary>
        /// Създава нова точка с координати N, E
        /// </summary>
        /// <param name="N">Northing (x)</param>
        /// <param name="E">Easting (y)</param>
        public Point(double N, double E)
            : this(N, E, 0.0) { }

        /// <summary>
        /// Създава нова точка с координати N, E, Z
        /// </summary>
        /// <param name="N">Northing (x)</param>
        /// <param name="E">Easting (y)</param>
        /// <param name="Z">Z (h)</param>
        public Point(double N, double E, double Z)
        {
            this.N = N;
            this.E = E;
            this.Z = Z;

            this.CoordinateSystem = CoordinateSystem.Unknown;

            // This should be tested:
            // Initializes the extent of this geometry. 
            // Once the extent is calculated it shouldn't change 
            // as it is part of the spatial index
            this._envelope = new Envelope(this.E, this.N, this.E, this.N);
        }

        /// <summary>
        /// Добавя "relativePoint" към "referencePoint": "referencePoint" + "relativePoint".
        /// </summary>
        /// <param name="referencePoint">Референтна точка</param>
        /// <param name="relativePoint">Стойността на <see cref="Point.GUID"/> се добавя към резултата</param>
        /// <returns></returns>
        public static Point operator +(Point referencePoint, Point relativePoint)
            => new Point(referencePoint.N + relativePoint.N, referencePoint.E + relativePoint.E, referencePoint.Z + relativePoint.Z)
            {
                GUID = relativePoint.GUID,
                PointInfo = relativePoint.PointInfo,
                CoordinateSystem = relativePoint.CoordinateSystem
            };

        /// <summary>
        /// Изважда "relativePoint" от "referencePoint": "referencePoint" - "relativePoint".
        /// </summary>
        /// <param name="referencePoint">Референтна точка</param>
        /// <param name="relativePoint">Стойността на <see cref="Point.GUID"/> се добавя към резултата
        public static Point operator -(Point referencePoint, Point relativePoint)
            => new Point(referencePoint.N - relativePoint.N, referencePoint.E - relativePoint.E, referencePoint.Z - relativePoint.Z)
            {
                GUID = relativePoint.GUID,
                PointInfo = relativePoint.PointInfo,
                CoordinateSystem = relativePoint.CoordinateSystem
            };

        /// <summary>
        /// Определя дали координатите на "thisPoint" са различни от координатите на "otherPoint".
        /// </summary>
        /// <param name="thisPoint"></param>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public static bool operator !=(Point thisPoint, Point otherPoint)
            => thisPoint.N != otherPoint.N || thisPoint.E != otherPoint.E || thisPoint.Z != otherPoint.Z;

        /// <summary>
        /// Определя дали координатите на "thisPoint" са равни на координатите на "otherPoint".
        /// </summary>
        /// <param name="thisPoint"></param>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public static bool operator ==(Point thisPoint, Point otherPoint)
            => thisPoint.N == otherPoint.N && thisPoint.E == otherPoint.E && thisPoint.Z == otherPoint.Z;

        /// <summary>
        /// Определя дали този обект е равен на "obj"
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => this.Equals(obj as Point);
        /// <summary>
        /// Определя дали тази точка е равна на "otherPoint"
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <returns></returns>
        public bool Equals(Point otherPoint)
        {
            if (otherPoint is null)
                return false;

            if (Object.ReferenceEquals(this, otherPoint))
                return true;

            if (this.GetType() != otherPoint.GetType())
                return false;

            return (this.N == otherPoint.N)
                && (this.E == otherPoint.E)
                && (this.Z == otherPoint.Z);
        }
        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => (N, E, Z).GetHashCode();
        /// <summary>
        /// Създава ново копие на този обект
        /// </summary>
        /// <returns></returns>
        public IPoint Clone()
            => new Point(this.N, this.E, this.Z)
            {
                PointInfo = this.PointInfo,
                CoordinateSystem = this.CoordinateSystem
            };
        /// <summary>
        /// Създава копие на тази точка
        /// </summary>
        /// <returns></returns>
        public IGeometry CopyGeometry()
            => this.Clone() as IGeometry;
    }
}
