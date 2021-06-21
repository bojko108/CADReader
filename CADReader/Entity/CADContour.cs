using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Globalization;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява площни обекти в CAD файл
    /// </summary>
    public class CADContour : ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; private set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// тип на контура
        /// </summary>
        public string Type => CADLineType.Get(this.t);
        /// <summary>
        /// идентификатор на контура, съгласно т.7
        /// </summary>
        public string CADId { get; private set; }
        /// <summary>
        /// координати на точка вътрешна за контура
        /// </summary>
        public Point InteriorPoint { get; set; }

        /// <summary>
        /// Дата на легалната поява на обекта
        /// </summary>
        public DateTime CreatedDate { get; private set; }
        /// <summary>
        /// Дата на преустановяване на легалното съществуване обекта
        /// </summary>
        public DateTime RemovedDate { get; private set; }

        /// <summary>
        /// Позиция на контура
        /// </summary>
        public IGeometry Geometry { get; private set; }

        /// <summary>
        /// код за тип на контура: сграда, поземлен имот, кадастрален район, землище 
        /// (използва се класификацията за параметър "k" от дефиницията на линиите т.9)
        /// </summary>
        private readonly int t;

        /// <summary>
        /// Създава нов контур
        /// </summary>
        /// <param name="values">t n х у b d</param>
        public CADContour(string[] values)
        {
            this.t = int.Parse(values[0]);
            this.CADId = values[1];

            DateTime date;
            if (DateTime.TryParseExact(values[4], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.CreatedDate = date;
            else
                this.RemovedDate = DateTime.MinValue;

            if (DateTime.TryParseExact(values[5], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
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
