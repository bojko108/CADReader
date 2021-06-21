using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Тип на маркера
    /// </summary>
    public static class CADPointMarkerType
    {
        /// <summary>
        /// Връща типа на маркера
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            return markers.ContainsKey(code)
                ? markers[code]
                : null;
        }

        private static readonly Dictionary<int, string> markers = new Dictionary<int, string>()
        {
            {0,"Липсва информация"},
            {1,"За маркиране на землищни, общински и обласни граници"},
            {2,"Железобетонни колове или тръби, поставени в земята"},
            {3,"Железобетонни колове или тръби с бетонова основа"},
            {4,"За маркиране на граници на поземлени имоти"},
            {5,"Знаци от камък и бетон"},
            {6,"Камък от твърда порода"},
            {7,"Метален прът с бетонирана горна част"},
            {8,"Бетоново блокче с метален прът или тръба"},
            {9,"Железобетонен или железен кол"},
            {10,"Знаци от други материали"},
            {11,"Тръби от твърда пластмаса"},
            {12,"Маркиране с жълта боя ъглите на масивни съоражения"},
            {13,"Знак от некорозираща алуминиева сплав"},
            {14,"Знак от компресиран полиетилен"},
            {15,"Знак от полимерен бетон"},
            {16,"Маркиращ болт"},
            {17,"Маркиращ пирон"},
            {18,"Маркиращи табели"}
        };
    }
}
