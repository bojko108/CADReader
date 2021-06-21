using RBush;
using System.Collections.Generic;

namespace CAD.Geometry
{
    /// <summary>
    /// За описване на геометрии на обекти в CAD файл
    /// </summary>
    public interface IGeometry : ISpatialData
    {
        /// <summary>
        /// Уникален идентификатор на обекта - използва се връзка между 
        /// геометрията от пространственият индекс и обекта от CAD файла
        /// </summary>
        string GUID { get; set; }
    }

    /// <summary>
    /// Допълнителни параметри за геометрията
    /// </summary>
    public interface IGeometryInfo
    {
        /// <summary>
        /// ДЪлжина на геометрията
        /// </summary>
        double Length { get; }
        /// <summary>
        /// Площ на геометрията
        /// </summary>
        double Area { get; }
    }
}
