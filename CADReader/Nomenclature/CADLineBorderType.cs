using System.Collections.Generic;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Тип на линията като граница
    /// </summary>
    public static class CADLineBorderType
    {
        /// <summary>
        /// Връща типа на линията като граница
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return lines.ContainsKey(code)
                ? lines[code]
                : null;
        }

        private static readonly Dictionary<int, string> lines = new Dictionary<int, string>()
        {
            {0,"Не е граница"},
            {1,"Граница с нисък приоритет"},
            {2,"Граница с основен приоритет(имот, подотдел, УПИ, почвена категория)"},
            {3,"Граница с висок приоритет(кадастрален район, отдел, квартал)"},
            {4,"Граница с по-висок приоритет(землищна граница)"}
        };
    }
}
