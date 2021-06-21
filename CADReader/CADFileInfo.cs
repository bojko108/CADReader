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
        public Envelope Window { get; set; }
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

        //private void SetProjection()
        //{
        //    // isntead of this, enumProjection should provide option for Unknown projection!!!
        //    this.Projection = enumProjection.WGS84_GEOGRAPHIC;


        //    // COORDTYPE t, h[, zon] - тип на използваната геодезическа координатна и височинна система:
        //    //           t = 1950 – координатна система 1950 г.
        //    //           t = 1970 – координатна система 1970 г. (по изискване на чл.4, ал.1 на Наредба14)
        //    //           h = Балтийска(по изискване на чл.4, ал.1 на Наредба 14)
        //    //           zon – зона(K3, K5, K7, K9) – задължителна само за КС1970

        //    string[] values = this._coordtype
        //        .Trim()
        //        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //    if ("1950".Equals(values[0]))
        //    {

        //    }
        //    if ("1970".Equals(values[0]))
        //    {
        //        switch (values[2])
        //        {
        //            case "K3":
        //                this.Projection = enumProjection.BGS_1970_K3;
        //                break;
        //            case "K5":
        //                this.Projection = enumProjection.BGS_1970_K5;
        //                break;
        //            case "K7":
        //                this.Projection = enumProjection.BGS_1970_K7;
        //                break;
        //            case "K9":
        //                this.Projection = enumProjection.BGS_1970_K9;
        //                break;
        //        }
        //    }
        //}
    }
}
