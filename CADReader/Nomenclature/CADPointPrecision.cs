using System.Collections.Generic;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Съдържа кодове за точност и техните стойности в метри
    /// </summary>
    public static class CADPointPrecision
    {
        /// <summary>
        /// Връща точност за подаденият код (по приложение №2)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return precision.ContainsKey(code)
                ? precision[code]
                : null;
        }

        private static readonly Dictionary<int, string> precision = new Dictionary<int, string>()
        {
            {11,"0.05 м"},
            {12,"0.10 м"},
            {13,"0.15 м"},
            {14,"0.2 м"},
            {15,"0.3 м"},
            {16,"0.4 м"},
            {17,"0.6 м"},
            {18,"0.9 м"},
            {19,"1.2 м"},
            {20,"1.8 м"},
            {21,"над 1.8 м"}
        };
    }
}
