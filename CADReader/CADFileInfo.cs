using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Linq;

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
        public CADVersion Version { get; private set; }
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
        public string Coordtype { get; private set; }
        /// <summary>
        /// Координатна система на файла. Ако е приложена трансформация на данните, тази стойност 
        /// ще се различава от <see cref="IGeometry.CoordinateSystem"/>. <see cref="CoordinateSystem"/> 
        /// ще посочва оригиналната координатна система в CAD файла, а <see cref="IGeometry.CoordinateSystem"/> 
        /// ще посочва новата.
        /// </summary>
        public CoordinateSystem CoordinateSystem { get; private set; }
        /// <summary>
        /// Съдържание на файла – служи за ориентация на софтуера за характера и обема на данните
        /// </summary>
        public CADContentType Contents { get; set; }
        /// <summary>
        /// Коментар – 128 символа
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Задава версията на CAD файла
        /// </summary>
        /// <param name="version">"4.00" или "4.03" ...</param>
        public void SetVersion(string version)
        {
            switch (version)
            {
                case "4.00":
                    this.Version = CADVersion.v400;
                    break;
                case "4.03":
                    this.Version = CADVersion.v403;
                    break;
                case "2.00":
                    this.Version = CADVersion.v200;
                    break;
                default:
                    this.Version = CADVersion.Unknown;
                    break;
            }
        }

        /// <summary>
        /// Задава координатната система на CAD файла
        /// </summary>
        /// <param name="coordtype"></param>
        public void SetCoordtype(string coordtype)
        {
            this.Coordtype = coordtype;

            string[] values = this.Coordtype
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToArray();

            switch (values[0])
            {
                case "1930":
                    switch (values[2])
                    {
                        case "8":
                            this.CoordinateSystem = CoordinateSystem.BGS_1930_24;
                            break;
                        case "9":
                            this.CoordinateSystem = CoordinateSystem.BGS_1930_27;
                            break;
                        default:
                            this.CoordinateSystem = CoordinateSystem.Unknown;
                            break;
                    }
                    break;
                case "1950":
                    switch (values[2])
                    {
                        case "8":
                            this.CoordinateSystem = CoordinateSystem.BGS_1950_3_24;
                            break;
                        case "9":
                            this.CoordinateSystem = CoordinateSystem.BGS_1950_3_27;
                            break;
                        case "4":
                            this.CoordinateSystem = CoordinateSystem.BGS_1950_6_21;
                            break;
                        case "5":
                            this.CoordinateSystem = CoordinateSystem.BGS_1950_6_27;
                            break;
                        default:
                            this.CoordinateSystem = CoordinateSystem.Unknown;
                            break;
                    }
                    break;
                case "1970":
                    switch (values[2])
                    {
                        case "K3":
                            this.CoordinateSystem = CoordinateSystem.BGS_1970_K3;
                            break;
                        case "K5":
                            this.CoordinateSystem = CoordinateSystem.BGS_1970_K5;
                            break;
                        case "K7":
                            this.CoordinateSystem = CoordinateSystem.BGS_1970_K7;
                            break;
                        case "K9":
                            this.CoordinateSystem = CoordinateSystem.BGS_1970_K9;
                            break;
                        default:
                            this.CoordinateSystem = CoordinateSystem.Unknown;
                            break;
                    }
                    break;
                case "2005 UTM":
                    switch (values[2])
                    {
                        case "34":
                            this.CoordinateSystem = CoordinateSystem.UTM34N;
                            break;
                        case "35":
                            this.CoordinateSystem = CoordinateSystem.UTM35N;
                            break;
                        default:
                            this.CoordinateSystem = CoordinateSystem.Unknown;
                            break;
                    }
                    break;
                case "2005":
                    this.CoordinateSystem = CoordinateSystem.BGS_2005_KK;
                    break;
                default:
                    this.CoordinateSystem = CoordinateSystem.Unknown;
                    break;
            }
        }
    }
}
