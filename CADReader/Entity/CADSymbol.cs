using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Globalization;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява условен знак - точкови, площни
    /// </summary>
    public class CADSymbol : ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта
        /// </summary>
        public string GUID { get; private set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Уникален номер на условния знак
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Tип на точката - <see cref="CADSymbolType"/>
        /// </summary>
        public string Type => CADSymbolType.Get(t);
        /// <summary>
        /// Ъгъл на завъртане
        /// </summary>
        public int Rotation { get; private set; }
        /// <summary>
        /// Дата на легалната поява на обекта
        /// </summary>
        public DateTime CreatedDate { get; private set; }
        /// <summary>
        /// Дата на преустановяване на легалното съществуване обекта
        /// </summary>
        public DateTime RemovedDate { get; private set; }

        /// <summary>
        /// Позиция на знака
        /// </summary>
        public IGeometry Geometry { get; private set; }

        /// <summary>
        /// Код за тип на условния знак по класификатора в приложение № 1
        /// </summary>
        private readonly int t;

        /// <summary>
        /// Създава нов условен знак
        /// </summary>
        /// <param name="values">t n х у a m b d</param>
        public CADSymbol(string[] values)
        {
            this.t = int.Parse(values[0]);
            this.Number = int.Parse(values[1]);
            this.Rotation = int.Parse(values[4]);

            this.Geometry = new Point(
                double.Parse(values[2]),
                double.Parse(values[3]));

            DateTime date;
            if (DateTime.TryParseExact(values[6], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.CreatedDate = date;
            else
                this.RemovedDate = DateTime.MinValue;

            if (DateTime.TryParseExact(values[7], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.RemovedDate = date;
            else
                this.RemovedDate = DateTime.MaxValue;
        }

        public void SetGeometry(IGeometry newGeometry)
        {
            newGeometry.GUID = this.GUID;
            this.Geometry = newGeometry;
        }
    }
}
