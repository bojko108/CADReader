namespace CAD
{
    /// <summary>
    /// Съдържа ключови думи в CAD файл v4
    /// </summary>
    internal class CADConstants
    {
        /// <summary>
        /// Начало на заглавният блок
        /// </summary>
        public const string HEADER_START = "HEADER";
        /// <summary>
        /// Версия на формата на файла
        /// </summary>
        public const string VERSION = "VERSION";
        /// <summary>
        /// Код по ЕКАТТЕ на населеното място
        /// </summary>
        public const string EKATTE = "EKATTE";
        /// <summary>
        /// Име на населеното място
        /// </summary>
        public const string NAME = "NAME";
        /// <summary>
        /// Име на програмната система
        /// </summary>
        public const string PROGRAM = "PROGRAM";
        /// <summary>
        /// [dd.MM.yyyy] - Дата на създаване на файла
        /// </summary>
        public const string DATE = "DATE";
        /// <summary>
        /// Име на фирмата изпълнител
        /// </summary>
        public const string FIRM = "FIRM";
        /// <summary>
        /// [x y] - Начало на редуцирана координатна система. 
        /// Всички координати във файла са относителни спрямо тази точка.
        /// </summary>
        public const string REFERENCE = "REFERENCE";
        /// <summary>
        /// [x1 y1 x2 y2] - Прозорец на графиката в редуцирани координати
        /// </summary>
        public const string WINDOW = "WINDOW";
        /// <summary>
        /// [t,h[,zon]] - Тип на използваната геодезическа координатна и височинна система:
        /// t=1950 – координатна система 1950 г.
        /// t=1970 – координатна система 1970 г. (по изискване на чл.4, ал.1 на Наредба14)
        /// h=Балтийска(по изискване на чл.4, ал.1 на Наредба 14)
        /// zon – зона(K3, K5, K7, K9) – задължителна само за КС1970
        /// </summary>
        public const string COORDTYPE = "COORDTYPE";
        /// <summary>
        /// [t] - Съдържание на файла – служи за ориентация на софтуера за характера и обема на данните:
        /// t= PRO проект за изменение на карта
        /// t=PART част от карта
        /// t=ALLK пълна карта
        /// </summary>
        public const string CONTENTS = "CONTENTS";
        /// <summary>
        /// Коментар – 128 символа
        /// </summary>
        public const string COMMENT = "COMMENT";
        /// <summary>
        /// Край на заглавният блок
        /// </summary>
        public const string END_HEADER = "END_HEADER";
        /// <summary>
        /// Начало на графичен блок
        /// </summary>
        public const string LAYER = "LAYER";
        /// <summary>
        /// Край на графичен блок
        /// </summary>
        public const string END_LAYER = "END_LAYER";
        /// <summary>
        /// Точка - служи за описание на елементи от геодезическата мрежа
        /// </summary>
        public const string P = "P";
        /// <summary>
        /// Линия - служи за описание на линейни елементи (линейни условни знаци)
        /// </summary>
        public const string L = "L";
        /// <summary>
        /// Контур - служи за описание на площни обекти
        /// </summary>
        public const string C = "C";
        /// <summary>
        /// Условен знак - служи за описание на условно изразени обекти - точкови
        /// </summary>
        public const string S = "S";
        /// <summary>
        /// Текст - служи за представяне на пояснителни надписи
        /// </summary>
        public const string T = "T";
        /// <summary>
        /// Разделител между точките
        /// </summary>
        public const string END_LINE = ";";
        /// <summary>
        /// Интервал
        /// </summary>
        public const string SEPARATOR = " ";
        /// <summary>
        /// Формат за дата
        /// </summary>
        public const string DATE_FORMAT = "dd.MM.yyyy";
        /// <summary>
        /// Формат за дата и време
        /// </summary>
        public const string DATETIME_FORMAT = "dd.MM.yyyy HH:mm";
    }
}
