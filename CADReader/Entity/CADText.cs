using CAD.Geometry;
using CAD.Nomenclature;
using System;
using System.Globalization;

namespace CAD.Entity
{
    /// <summary>
    /// Представлява текст, който служи за представяне на пояснителни надписи в CAD файла
    /// </summary>
    public class CADText : ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        public string GUID { get; private set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Уникален номер на текста
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Tип на текста - <see cref="CADTextType"/>
        /// </summary>
        public string Type => CADTextType.Get(t);
        /// <summary>
        /// височина на надписа в милиметри на хартията
        /// </summary>
        public double Height { get; private set; }
        /// <summary>
        /// Ъгъл на завъртане на текста (100 гона за хоризонтален). 
        /// </summary>
        public int Rotation { get; private set; }
        /// <summary>
        /// Подравняване на текста
        /// </summary>
        public CADTextAlignment Alignment { get; private set; }
        /// <summary>
        /// Надпис
        /// </summary>
        public string Text
        {
            get
            {
                return this.ps;

                /*
                 * should return:
                 * {ps} {tt} {ss}
                 */
            }
        }

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
        /// тип на текста по класификатора в приложение № 1
        /// </summary>
        private readonly int t;
        /// <summary>
        /// Двубуквен код за подравняване – първата буква за хоризонтално 
        /// (L – ляво, C – централно, R - дясно), втората за вертикални 
        /// (Т – горе, C – централно, D – долу)
        /// </summary>
        private readonly string j;
        /// <summary>
        /// префиксен свободен текст
        /// </summary>
        private string ps;
        /// <summary>
        /// тип на обекта
        /// </summary>
        private TextType tt;
        /// <summary>
        /// уникален номер на графичен обект, идентификатор на имота или ключ в таблица
        /// </summary>
        private string n;
        /// <summary>
        /// графичен параметър (код) - <see cref="TextContentType"/>
        /// </summary>
        private string p;
        /// <summary>
        /// суфиксен свободен текст
        /// </summary>
        private string ss;

        /// <summary>
        /// Създава нов текст
        /// </summary>
        /// <param name="values">t n х у h b d r j</param>
        public CADText(string[] values)
        {
            this.t = int.Parse(values[0]);
            this.Number = int.Parse(values[1]);
            this.Height = double.Parse(values[4]);
            this.j = values[8];
            switch (this.j)
            {
                case "LT":
                    this.Alignment = CADTextAlignment.TopLeft;
                    break;
                case "LC":
                    this.Alignment = CADTextAlignment.MiddleLeft;
                    break;
                case "LD":
                    this.Alignment = CADTextAlignment.BottomLeft;
                    break;
                case "CT":
                    this.Alignment = CADTextAlignment.TopCenter;
                    break;
                case "CC":
                    this.Alignment = CADTextAlignment.MiddleCenter;
                    break;
                case "CD":
                    this.Alignment = CADTextAlignment.BottomCenter;
                    break;
                case "RT":
                    this.Alignment = CADTextAlignment.TopRight;
                    break;
                case "RC":
                    this.Alignment = CADTextAlignment.MiddleRight;
                    break;
                case "RD":
                    this.Alignment = CADTextAlignment.BottomRight;
                    break;
                default:
                    this.Alignment = CADTextAlignment.BottomLeft;
                    break;
            }

            this.Geometry = new Point(
                double.Parse(values[2]),
                double.Parse(values[3]));

            DateTime date;
            if (DateTime.TryParseExact(values[5], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.CreatedDate = date;
            else
                this.RemovedDate = DateTime.MinValue;

            if (DateTime.TryParseExact(values[6], CADConstants.DATE_FORMAT, null, DateTimeStyles.None, out date))
                this.RemovedDate = date;
            else
                this.RemovedDate = DateTime.MaxValue;
        }

        /// <summary>
        /// Задава текста и допълнителни параметри.
        /// </summary>
        /// <param name="values"><[ps] [t n p] [ss]</param>
        public void SetText(string values)
        {
            this.ps = values;
            this.ps.Trim('"', (char)CADConstants.SEPARATOR[0]);

            //if (values.Length == 5)
            //{
            //    this.tt = (TextType)Enum.Parse(typeof(TextType), values[1]);
            //    this.n = values[2];
            //    this.p = values[3];
            //    this.ss = values[4];
            //    this.ss.Trim('"', (char)CADConstants.SEPARATOR[0]);
            //}
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
