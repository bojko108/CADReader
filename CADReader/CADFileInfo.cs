using CAD.Geometry;
using CAD.Nomenclature;
using RBush;
using System;

namespace CAD
{
    /// <summary>
    /// Съдържа описателна информация за CAD файл v4
    /// </summary>
    public class CADFileInfo
    {
        /// <summary>
        /// Версия на формата на файла
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Код по ЕКАТТЕ на населеното място
        /// </summary>
        public string EKATTE { get; set; }
        /// <summary>
        /// Име на населеното място
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Име на програмната система
        /// </summary>
        public string Program { get; set; }
        /// <summary>
        /// [dd.MM.yyyy] - Дата на създаване на файла
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Име на фирмата изпълнител
        /// </summary>
        public string Firm { get; set; }
        /// <summary>
        /// Начало на редуцирана координатна система. 
        /// Всички координати във файла са относителни спрямо тази точка.
        /// </summary>
        public Point ReferencePoint { get; set; }
        /// <summary>
        /// [x1 y1 x2 y2] - Прозорец на графиката в редуцирани координати
        /// </summary>
        public Extent Window { get; set; }
        /// <summary>
        /// Тип на използваната геодезическа координатна и височинна система
        /// </summary>
        public string Coordtype { get; set; }
        /// <summary>
        /// Съдържание на файла – служи за ориентация на софтуера за характера и обема на данните
        /// </summary>
        public CADContentType Contents { get; set; }
        /// <summary>
        /// Коментар – 128 символа
        /// </summary>
        public string Comment { get; set; }
    }
}
