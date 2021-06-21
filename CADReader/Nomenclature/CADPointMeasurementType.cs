using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Метод на определяне на подробни точки
    /// </summary>
    public static class CADPointMeasurementType
    {
        /// <summary>
        /// Връща метода на определяне на подробна точка
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return methods.ContainsKey(code)
                ? methods[code]
                : null;
        }

        private static readonly Dictionary<int, string> methods = new Dictionary<int, string>()
        {
            {0,"Не е граница"},
            {1,"Граница с нисък приоритет"},
            {2,"Граница с основен приоритет(имот, подотдел, УПИ, почвена категория)"},
            {3,"Граница с висок приоритет(кадастрален район, отдел, квартал)"},
            {4,"Граница с по-висок приоритет(землищна граница)"}
        };
    }
}
