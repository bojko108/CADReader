using System.Collections.Generic;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Номенклатура за начина на стабилизиране на точки от геодезическата основа
    /// </summary>
    public static class CADPointMarking
    {
        /// <summary>
        /// Връща начина на стабилизиране на точки от геодезическата основа
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return marking.ContainsKey(code)
                ? marking[code]
                : null;
        }

        private static readonly Dictionary<int, string> marking = new Dictionary<int, string>()
        {
            {0,"Липсва информация"},
            {1,"Знаци от камък и бетон"},
            {2,"Камък от твърда порода"},
            {3,"Метална тръба с бетонирана горна част"},
            {4,"Бетоново блокче с метална тръба в центъра"},
            {5,"Знаци от полимерен бетон"},
            {6,"Знаци от полимерен бетон с 'С' образен ствол"},
            {7,"Знаци от други материали"},
            {8,"Знаци за стабилизиране върху трайни настилки"},
            {9,"Метална тръба"},
            {10,"Маркиращ пирон"}
        };
    }
}
