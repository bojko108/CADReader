using CAD.Geometry;
using CAD.Nomenclature;

namespace CAD.Entity
{
    /// <summary>
    /// Съдържа информация за точка от линия, <see cref="Point"/>
    /// </summary>
    public class PointInfo
    {
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
        /// Създава информация за точка от линия
        /// </summary>
        /// <param name="values">nl xl yl tl o1 m2</param>
        public PointInfo(string[] values)
        {
            this.Number = int.Parse(values[0]);

            this.t = int.Parse(values[3]);
            this.o = int.Parse(values[4]);
            this.m = int.Parse(values[5]);
        }
    }
}
