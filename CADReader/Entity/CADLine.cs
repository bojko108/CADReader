using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Globalization;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява линейни елементи в CAD файл
    /// </summary>
    public class CADLine : ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; private set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Тип на линията - <see cref="CADLineType"/>
        /// </summary>
        public string Type => CADLineType.Get(this.t);
        /// <summary>
        /// Уникален номер на линията
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Tип на линията като граница - <see cref="CADLineBorderType"/>
        /// </summary>
        public string BorderType => CADLineBorderType.Get(this.k);
        /// <summary>
        /// Дата на легалната поява на обекта
        /// </summary>
        public DateTime CreatedDate { get; private set; }
        /// <summary>
        /// Дата на преустановяване на легалното съществуване обекта
        /// </summary>
        public DateTime RemovedDate { get; private set; }

        /// <summary>
        /// код за тип на линията по класификатора в т. 1.2 от приложение № 1
        /// </summary>
        private readonly int t;
        /// <summary>
        /// Код за тип на линията като граница
        /// </summary>
        private readonly int k;
        /// <summary>
        /// допълнителен параметър - надморска височина на линията, 
        /// когато се описват линии с еднаква височина или измерена дължина 
        /// на линията, когато се описва репераж.
        /// </summary>
        private readonly double h;

        /// <summary>
        /// Позиция на линията
        /// </summary>
        public IGeometry Geometry { get; private set; }

        /// <summary>
        /// Създаване на нова линия
        /// </summary>
        /// <param name="values">t n k b d {h}</param>
        public CADLine(string[] values)
        {
            this.t = int.Parse(values[0]);
            this.Number = int.Parse(values[1]);
            this.k = int.Parse(values[2]);

            DateTime date;
            if (DateTime.TryParseExact(values[3], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.CreatedDate = date;
            else
                this.RemovedDate = DateTime.MinValue;

            if (DateTime.TryParseExact(values[4], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.RemovedDate = date;
            else
                this.RemovedDate = DateTime.MaxValue;

            if (values.Length == 6)
                this.h = double.Parse(values[5]);
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
