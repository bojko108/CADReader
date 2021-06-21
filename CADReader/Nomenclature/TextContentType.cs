using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Nomenclature
{
    /// <summary>
    /// 
    /// </summary>
    public static class TextContentType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(string code)
        {
            return codes.ContainsKey(code)
                ? codes[code]
                : null;
        }

        private static readonly Dictionary<string, string> codes = new Dictionary<string, string>()
        {
            {"AN","административен номер на обект"},
            {"SI","сигнатура на сграда"},
            {"NU","номер на обект"},
            {"LE","дължина"},
            {"ХС","х координата"},
            {"YC","у координата"},
            {"HI","височина"},
            {"AR","площ"},
            {"LP","номер на точка от линия"},
            {"AD","надпис на адрес"},
            {"ST","надпис на улица"},
            {"IO","данни за собственика/ците на контур (идентификатор на контур)"}
        };
    }
}
