using CAD.Geometry;

namespace CAD.Entity
{
    /// <summary>
    /// Графика от CAD файл - <see cref="CADPoint"/>, <see cref="CADLine"/>, <see cref="CADContour"/>, 
    /// <see cref="CADSymbol"/>, <see cref="CADText"/>
    /// </summary>
    public interface ICADEntity
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        string GUID { get; }
        /// <summary>
        /// Геометрия на обекта
        /// </summary>
        IGeometry Geometry { get; }
        /// <summary>
        /// Използва се за промяна на геометрията на обекта
        /// </summary>
        /// <param name="newGeometry"></param>
        void SetGeometry(IGeometry newGeometry);
    }
}
