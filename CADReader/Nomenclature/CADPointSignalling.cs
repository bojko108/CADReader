using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Номенклатура за начина на сигнализиране на точки от геодезическата основа
    /// </summary>
    public static class CADPointSignalling
    {
        /// <summary>
        /// Връща начина на сигнализиране на точки от геодезическата основа
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return signals.ContainsKey(code)
                ? signals[code]
                : null;
        }

        private static readonly Dictionary<int, string> signals = new Dictionary<int, string>()
        {
            {0,"Няма"},
            {1,"Метална пирамида"},
            {2,"Дървена веха"}
        };
    }
}
