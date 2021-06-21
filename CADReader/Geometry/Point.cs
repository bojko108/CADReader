using CAD.Nomenclature;
using RBush;
using System;

namespace CAD.Geometry
{
    /// <summary>
    /// Представлява точка
    /// </summary>
    public class Point : IGeometry, IEquatable<Point>
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// Уникален номер на линията
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Точност на точката - <see cref="CADPointPrecision"/>
        /// </summary>
        public string Precision => CADPointPrecision.Get(this.t);
        /// <summary>
        /// Трайно означаване - <see cref="CADPointMarkerType"/>
        /// </summary>
        public string MarkerType => CADPointMarkerType.Get(this.o);
        /// <summary>
        /// Метод на определяне - <see cref="CADPointMeasurementType"/>
        /// </summary>
        public string MeasurementType => CADPointMeasurementType.Get(this.m);
        /// <summary>
        /// Northing в метри (x в CAD файл)
        /// </summary>
        public double N { get; private set; }
        /// <summary>
        /// Easting в метри (y в CAD файл)
        /// </summary>
        public double E { get; private set; }
        /// <summary>
        /// Z в метри (h в CAD файл)
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// Обхват на геометрията - служи при търсене в пространственият индекс
        /// </summary>
        public ref readonly Envelope Envelope => ref _envelope;
        private readonly Envelope _envelope;

        /// <summary>
        /// Код за точност на точката (приложение №2)
        /// </summary>
        private readonly int t;
        /// <summary>
        /// Код за трайно означаване (обр.0220 от Нар.14) 
        /// (номенклатура за трайно означаване)
        /// </summary>
        private readonly int o;
        /// <summary>
        /// Код за метод на определяне (обр.0470 от Нар.14) 
        /// (номенклатура за метод на определяне)
        /// </summary>
        private readonly int m;

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

            // This should be tested:
            // Initializes the extent of this geometry. 
            // Once the extent is calculated it shouldn't change 
            // as it is part of the spatial index
            this._envelope = new Envelope(this.E, this.N, this.E, this.N);
        }

        /// <summary>
        /// Създава нова точка по данни от CAD файл
        /// </summary>
        /// <param name="values">nl xl yl tl o1 m2</param>
        public Point(string[] values)
        {
            this.Number = int.Parse(values[0]);

            this.N = double.Parse(values[1]);
            this.E = double.Parse(values[2]);
            this.Z = 0.0;

            this._envelope = new Envelope(this.E, this.N, this.E, this.N);

            this.t = int.Parse(values[3]);
            this.o = int.Parse(values[4]);
            this.m = int.Parse(values[5]);
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
                GUID = relativePoint.GUID
            };

        /// <summary>
        /// Изважда "relativePoint" от "referencePoint": "referencePoint" - "relativePoint".
        /// </summary>
        /// <param name="referencePoint">Референтна точка</param>
        /// <param name="relativePoint">Стойността на <see cref="Point.GUID"/> се добавя към резултата
        public static Point operator -(Point referencePoint, Point relativePoint)
            => new Point(referencePoint.N - relativePoint.N, referencePoint.E - relativePoint.E, referencePoint.Z - relativePoint.Z)
            {
                GUID = relativePoint.GUID
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
    }
}
