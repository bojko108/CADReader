using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Globalization;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява точка от геодезическа мрежа
    /// </summary>
    public class CADPoint : ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; private set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Tип на точката - <see cref="CADPointType"/>
        /// </summary>
        public string Type => CADPointType.Get(t);
        /// <summary>
        /// Уникален номер на точката
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Клас по положение на точката
        /// </summary>
        public int GClass { get; private set; }
        /// <summary>
        /// Точност по X
        /// </summary>
        public double MX { get; private set; }
        /// <summary>
        /// Точност по Y
        /// </summary>
        public double MY { get; private set; }
        /// <summary>
        /// Клас по височина на точката
        /// </summary>
        public int HClass { get; private set; }
        /// <summary>
        /// Точност по височина
        /// </summary>
        public double MH { get; private set; }
        /// <summary>
        /// Начин на стабилизиране - <see cref="CADPointMarking"/>
        /// </summary>
        public string Marking => CADPointMarking.Get(this.mst);
        /// <summary>
        /// Начин на сигнализиране - <see cref="CADPointSignalling"/>
        /// </summary>
        public string Signalling => CADPointSignalling.Get(this.msg);
        /// <summary>
        /// Номер на знак
        /// </summary>
        public int Sign { get; private set; }
        /// <summary>
        /// Подземен център (False – няма, True – има)
        /// </summary>
        public bool UndergroundMarker => 1 == this.cen;
        /// <summary>
        /// Стар номер, ограден в кавички, включително и картния лист
        /// </summary>
        public string OldNumber { get; private set; }
        /// <summary>
        /// Дата на легалната поява на обекта
        /// </summary>
        public DateTime CreatedDate { get; private set; }
        /// <summary>
        /// Дата на преустановяване на легалното съществуване обекта
        /// </summary>
        public DateTime RemovedDate { get; private set; }

        /// <summary>
        /// Позиция на точката
        /// </summary>
        public IGeometry Geometry { get; private set; }

        /// <summary>
        /// Код за тип на точката по класификатора в приложение №1
        /// </summary>
        private readonly int t;
        /// <summary>
        /// Код за начин на стабилизиране (номенклатура за начин на стабилизиране)
        /// </summary>
        private readonly int mst;
        /// <summary>
        /// Код за начин на сигнализиране (номенклатура за начин на сигнализиране)
        /// </summary>
        private readonly int msg;
        /// <summary>
        /// Код за подземен център (0 – няма, 1 – има)
        /// </summary>
        private readonly int cen;

        /// <summary>
        /// Създава нова точка
        /// </summary>
        /// <param name="values">t n х у h k mx my kh mh mst msg sgn cen ono b d</param>
        public CADPoint(string[] values)
        {
            this.t = int.Parse(values[0]);
            this.Number = int.Parse(values[1]);

            this.Geometry = new Point(
                double.Parse(values[2]),
                double.Parse(values[3]),
                double.Parse(values[4]))
            {
                GUID = this.GUID
            };

            this.GClass = int.Parse(values[5]);
            this.MX = double.Parse(values[6]);
            this.MY = double.Parse(values[7]);
            this.HClass = int.Parse(values[8]);
            this.MH = double.Parse(values[9]);
            this.mst = int.Parse(values[10]);
            this.msg = int.Parse(values[11]);
            this.Sign = int.Parse(values[12]);
            this.cen = int.Parse(values[13]);
            this.OldNumber = values[14];

            DateTime date;
            if (DateTime.TryParseExact(values[15], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.CreatedDate = date;
            else
                this.RemovedDate = DateTime.MinValue;

            if (DateTime.TryParseExact(values[16], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.RemovedDate = date;
            else
                this.RemovedDate = DateTime.MaxValue;
        }

        /// <summary>
        /// Задаване геометрията на обекта
        /// </summary>
        /// <param name="newGeometry"></param>
        public void SetGeometry(IGeometry newGeometry)
        {
            newGeometry.GUID = this.GUID;
            this.Geometry = newGeometry;
        }
    }
}
