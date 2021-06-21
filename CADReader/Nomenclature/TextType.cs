using CAD.Entity;

namespace CAD.Nomenclature
{
    /// <summary>
    /// Тип на обекта, за който е предназначен текста
    /// </summary>
    public enum TextType
    {
        /// <summary>
        /// Точка - <see cref="CADPoint"/>
        /// </summary>
        P,
        /// <summary>
        /// Линия - <see cref="CADLine"/>
        /// </summary>
        L,
        /// <summary>
        /// Контур - <see cref="CADContour"/>
        /// </summary>
        C,
        /// <summary>
        /// Условен знак - <see cref="CADSymbol"/>
        /// </summary>
        S,
        /// <summary>
        /// атрибут (запис в таблица???)
        /// </summary>
        A
    }
}
