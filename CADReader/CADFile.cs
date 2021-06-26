using CAD.Entity;
using CAD.Geometry;
using CAD.Internals;
using CAD.Nomenclature;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CAD
{
    /// <summary>
    /// Създава нов обект за четене на CAD файл v4
    /// </summary>
    public class CADFile
    {
        /// <summary>
        /// Описателна инфромация за CAD файла
        /// </summary>
        public CADFileInfo FileInfo { get; private set; }
        /// <summary>
        /// Отделните графични блокове в CAD файла
        /// </summary>
        public List<CADLayer> Layers { get; private set; }
        /// <summary>
        /// Достъп до съобщенията, генерирани по време на четене на файла
        /// </summary>
        public List<string> Log => Logger.Instance.Messages;

        private readonly Stream cadFile;

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="filePath">път до файла</param>
        public CADFile(string filePath)
            : this(filePath, CoordinateSystem.Unknown)
        { }

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="filePath">път до файла</param>
        /// <param name="target">целева координатна система</param>
        public CADFile(string filePath, CoordinateSystem target)
        {
            if (File.Exists(filePath) == false)
                throw new ArgumentException($"File does not exist or is not accessible: {filePath}");

            this.cadFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            TransformationUtils.Instance.SourceProjection = CoordinateSystem.Unknown;
            TransformationUtils.Instance.TargetProjection = target;
        }

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="fileContents">съдържание на CAD файла</param>
        public CADFile(byte[] fileContents)
            : this(fileContents, CoordinateSystem.Unknown)
        { }

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="fileContents">съдържание на CAD файла</param>
        /// <param name="target">целева координатна система</param>
        public CADFile(byte[] fileContents, CoordinateSystem target)
        {
            this.cadFile = new MemoryStream(fileContents);

            TransformationUtils.Instance.SourceProjection = CoordinateSystem.Unknown;
            TransformationUtils.Instance.TargetProjection = target;
        }

        /// <summary>
        /// Достъп до определен слой от CAD файла
        /// </summary>
        /// <param name="layer"><see cref="CADLayerType.CADASTER"/>, <see cref="CADLayerType.LESO"/>, 
        /// <see cref="CADLayerType.POCHKATEG"/>, <see cref="CADLayerType.REGPLAN"/> или
        /// <see cref="CADLayerType.SHEMI"/></param>
        /// <returns></returns>
        public CADLayer this[CADLayerType layer]
            => this.Layers.FirstOrDefault(l => layer == l.Type);

        /// <summary>
        /// Четене на CAD файла
        /// </summary>
        /// <param name="encoding"><see cref="Encoding.UTF8"/> by default</param>
        /// <exception cref = "System.Exception">Thrown if there is a problem reading the HEADER section 
        /// of the CAD file or the CAD file cannot be accesed</exception>
        public void ReadFile(Encoding encoding = null)
        {
            try
            {
                Logger.Instance.Reset();
                Logger.Instance.LogInfo($"Reading CAD file: length={this.cadFile.Length}");

                this.FileInfo = new CADFileInfo();
                this.Layers = new List<CADLayer>();

                using (StreamReader sr = new StreamReader(this.cadFile, encoding ?? Encoding.UTF8))
                {
                    // these points are needed for calculating transformation parameters
                    // after the COORDTYPE parameter is set
                    Point southWest = null, northEast = null;

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            continue;
                        line.Trim();

                        string[] values;
                        int separatorIndex;
                        string key;

                        #region Read HEADER section

                        try
                        {
                            if (CADConstants.HEADER_START.Equals(line))
                            {
                                while (CADConstants.END_HEADER.Equals(line) == false)
                                {
                                    line = sr.ReadLine();
                                    if (string.IsNullOrEmpty(line))
                                        continue;
                                    line.Trim();

                                    separatorIndex = line.IndexOf(CADConstants.SEPARATOR);
                                    if (separatorIndex < 1)
                                        continue;
                                    key = line.Substring(0, separatorIndex);

                                    switch (key)
                                    {
                                        case CADConstants.VERSION:
                                            this.FileInfo.Version = line.Substring(separatorIndex + 1);
                                            if (this.FileInfo.Version.StartsWith("4") == false)
                                                throw new ArgumentException("Only CAD files v4 are supported", "file");
                                            continue;
                                        case CADConstants.EKATTE:
                                            this.FileInfo.EKATTE = line.Substring(separatorIndex + 1);
                                            continue;
                                        case CADConstants.NAME:
                                            this.FileInfo.Name = line.Substring(separatorIndex + 1);
                                            continue;
                                        case CADConstants.PROGRAM:
                                            this.FileInfo.Program = line.Substring(separatorIndex + 1);
                                            continue;
                                        case CADConstants.DATE:
                                            this.FileInfo.Date = DateTime.ParseExact(line
                                                .Substring(separatorIndex + 1), CADConstants.DATE_FORMAT, CultureInfo.InvariantCulture);
                                            continue;
                                        case CADConstants.FIRM:
                                            this.FileInfo.Firm = line.Substring(separatorIndex + 1);
                                            continue;
                                        case CADConstants.REFERENCE:
                                            values = line
                                                .Substring(separatorIndex + 1)
                                                .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            this.FileInfo.ReferencePoint = new Point(
                                                double.Parse(values[0]),
                                                double.Parse(values[1]));
                                            continue;
                                        case CADConstants.WINDOW:
                                            values = line
                                                .Substring(separatorIndex + 1)
                                                .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                            southWest = new Point(
                                                double.Parse(values[0]),
                                                double.Parse(values[1]));
                                            southWest = this.CalculateAbsoluteCoordinatesFor((Point)southWest);
                                            northEast = new Point(
                                                double.Parse(values[2]),
                                                double.Parse(values[3]));
                                            northEast = this.CalculateAbsoluteCoordinatesFor((Point)northEast);
                                            continue;
                                        case CADConstants.COORDTYPE:
                                            this.FileInfo.Coordtype = line.Substring(separatorIndex + 1);

                                            Extent extent = new Extent(
                                                southWest.N,
                                                southWest.E,
                                                northEast.N,
                                                northEast.E);

                                            TransformationUtils.Instance.SourceProjection = this.SetSourceProjection(this.FileInfo.Coordtype);
                                            TransformationUtils.Instance.CalculateTransformationParameters(extent);

                                            continue;
                                        case CADConstants.CONTENTS:
                                            this.FileInfo.Contents = (CADContentType)Enum.Parse(typeof(CADContentType), line
                                                .Substring(separatorIndex + 1));
                                            continue;
                                        case CADConstants.COMMENT:
                                            this.FileInfo.Comment = line.Substring(separatorIndex + 1);
                                            continue;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // critical error while reading the HEADER section
                            Logger.Instance.LogError(ex);
                            throw ex;
                        }

                        #endregion

                        #region Read GRAPHICS section

                        try
                        {
                            if (line.StartsWith(CADConstants.LAYER))
                            {
                                separatorIndex = line.IndexOf(CADConstants.SEPARATOR);
                                if (separatorIndex < 1)
                                    continue;

                                // create new layer for storing graphics
                                CADLayer layer = new CADLayer(line.Substring(separatorIndex + 1));

                                // FOR LINES AND CONTOURS: we don't know when the current entity ends
                                // so the current line needs to be saved so it can be used thereafter
                                string PREVIOUS_LINE = null;

                                while (CADConstants.END_LAYER.Equals(line) == false)
                                {
                                    try
                                    {
                                        // FOR LINES AND CONTOURS: when the current entity ends, the previous line
                                        // is set as current line - from previous iteration
                                        line = PREVIOUS_LINE ?? sr.ReadLine();
                                        PREVIOUS_LINE = null;

                                        if (string.IsNullOrEmpty(line))
                                            continue;
                                        line.Trim();

                                        separatorIndex = line.IndexOf(CADConstants.SEPARATOR);
                                        if (separatorIndex < 1)
                                            continue;
                                        key = line.Substring(0, separatorIndex);

                                        // read individual entities
                                        switch (key)
                                        {
                                            // GEO POINT entity
                                            case CADConstants.P:
                                                values = line
                                                    .Substring(separatorIndex + 1)
                                                    .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                ICADEntity geoPoint = new CADPoint(values);
                                                // first calculate the absolute coordinates using this.FileInfo.ReferencePoint
                                                // then transform the coordinates
                                                Point tmpPoint1 = this.CalculateAbsoluteCoordinatesFor(geoPoint.Geometry as Point);
                                                tmpPoint1 = TransformationUtils.Instance.TransformPoint(tmpPoint1);
                                                geoPoint.SetGeometry(tmpPoint1);

                                                layer.AddEntity(geoPoint);
                                                continue;

                                            // LINE entity
                                            case CADConstants.L:
                                                values = line
                                                    .Substring(separatorIndex + 1)
                                                    .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                CADLine geoLine = new CADLine(values);

                                                #region Read line vertices

                                                List<Point> vertices = new List<Point>();

                                                while (true)
                                                {
                                                    line = sr.ReadLine();
                                                    if (string.IsNullOrEmpty(line))
                                                        break;
                                                    line.Trim();
                                                    key = line.Substring(0, separatorIndex);
                                                    if (CADConstants.L.Equals(key)
                                                        || CADConstants.C.Equals(key)
                                                        || CADConstants.S.Equals(key)
                                                        || CADConstants.T.Equals(key)
                                                        || CADConstants.P.Equals(key)
                                                        || CADConstants.END_LAYER.Equals(key))
                                                    {
                                                        // saves current line for the next iteration
                                                        // CADLine has ended and we are now reading
                                                        // the next entity from the CAD file
                                                        PREVIOUS_LINE = line;
                                                        break;
                                                    }

                                                    values = line
                                                        .Split(new string[] { CADConstants.END_LINE }, StringSplitOptions.RemoveEmptyEntries);

                                                    foreach (string pointData in values)
                                                    {
                                                        pointData.Trim();
                                                        if (string.IsNullOrWhiteSpace(pointData))
                                                            continue;

                                                        string[] pointInfoValues = pointData
                                                           .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                        PointInfo info = new PointInfo(pointInfoValues);

                                                        Point tmpPoint2 = this.CalculateAbsoluteCoordinatesFor(new Point(
                                                            double.Parse(pointInfoValues[1]),
                                                            double.Parse(pointInfoValues[2])));
                                                        tmpPoint2 = TransformationUtils.Instance.TransformPoint(tmpPoint2);
                                                        tmpPoint2.PointInfo = info;

                                                        vertices.Add(tmpPoint2);
                                                    }
                                                }

                                                Polyline polyline = new Polyline(vertices);
                                                geoLine.SetGeometry(polyline);

                                                #endregion

                                                layer.AddEntity(geoLine);
                                                continue;

                                            // CONTOUR entity
                                            case CADConstants.C:
                                                values = line
                                                    .Substring(separatorIndex + 1)
                                                    .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                ICADEntity contour = new CADContour(values);

                                                Point interiorPoint = new Point(
                                                    double.Parse(values[2]),
                                                    double.Parse(values[3]));
                                                interiorPoint = this.CalculateAbsoluteCoordinatesFor(interiorPoint);
                                                interiorPoint = TransformationUtils.Instance.TransformPoint(interiorPoint);
                                                (contour as CADContour).InteriorPoint = interiorPoint;

                                                #region Read Contour lines

                                                List<Point> verticesContour = new List<Point>();

                                                while (true)
                                                {
                                                    line = sr.ReadLine();
                                                    if (string.IsNullOrEmpty(line))
                                                        break;
                                                    line.Trim();
                                                    key = line.Substring(0, separatorIndex);
                                                    if (CADConstants.L.Equals(key)
                                                        || CADConstants.C.Equals(key)
                                                        || CADConstants.S.Equals(key)
                                                        || CADConstants.T.Equals(key)
                                                        || CADConstants.P.Equals(key)
                                                        || CADConstants.END_LAYER.Equals(key))
                                                    {
                                                        // saves current line for the next iteration
                                                        // CADContour has ended and we are now reading
                                                        // the next entity from the CAD file
                                                        PREVIOUS_LINE = line;
                                                        break;
                                                    }

                                                    values = line
                                                        .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                    foreach (string lineNumber in values)
                                                    {
                                                        lineNumber.Trim();
                                                        if (string.IsNullOrWhiteSpace(lineNumber))
                                                            continue;

                                                        CADLine lineContour = layer.Search(e =>
                                                        {
                                                            if (e is CADLine l)
                                                                return lineNumber.Equals(l.Number.ToString());
                                                            else
                                                                return false;
                                                        }).FirstOrDefault() as CADLine;

                                                        if (lineContour == null)
                                                            continue;

                                                        verticesContour.AddRange((lineContour.Geometry as Polyline).Vertices);
                                                    }
                                                }

                                                Polygon polygon = new Polygon(verticesContour);
                                                contour.SetGeometry(polygon);

                                                #endregion

                                                layer.AddEntity(contour);
                                                continue;

                                            // SYMBOL entity
                                            case CADConstants.S:
                                                values = line
                                                    .Substring(separatorIndex + 1)
                                                    .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                ICADEntity symbol = new CADSymbol(values);
                                                Point tmpPoint3 = this.CalculateAbsoluteCoordinatesFor(symbol.Geometry as Point);
                                                tmpPoint3 = TransformationUtils.Instance.TransformPoint(tmpPoint3);
                                                symbol.SetGeometry(tmpPoint3);

                                                layer.AddEntity(symbol);
                                                continue;

                                            // TEXT entity
                                            case CADConstants.T:
                                                values = line
                                                     .Substring(separatorIndex + 1)
                                                     .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                                CADText text = new CADText(values);
                                                Point tmpPoint4 = this.CalculateAbsoluteCoordinatesFor(text.Geometry as Point);
                                                tmpPoint4 = TransformationUtils.Instance.TransformPoint(tmpPoint4);
                                                text.SetGeometry(tmpPoint4);

                                                line = sr.ReadLine();
                                                if (string.IsNullOrEmpty(line))
                                                    continue;
                                                line.Trim();
                                                text.SetText(line);

                                                layer.AddEntity(text);
                                                continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // while reading entities
                                        Logger.Instance.LogError(ex);
                                    }
                                }

                                this.Layers.Add(layer);
                            }
                        }
                        catch (Exception ex)
                        {
                            // while reading a layer
                            Logger.Instance.LogError(ex);
                        }

                        #endregion
                    }

                    try
                    {
                        if (TransformationUtils.Instance.NeedsTransformation)
                        {
                            this.FileInfo.ReferencePoint = TransformationUtils.Instance.TransformPoint(this.FileInfo.ReferencePoint);

                            southWest = TransformationUtils.Instance.TransformPoint(southWest);
                            northEast = TransformationUtils.Instance.TransformPoint(northEast);
                        }

                        this.FileInfo.ReferencePoint.CoordinateSystem = TransformationUtils.Instance.TargetProjection;

                        this.FileInfo.Window = new Extent(
                             southWest.N,
                             southWest.E,
                             northEast.N,
                             northEast.E);
                    }
                    catch (Exception ex)
                    {
                        // while calculating reference point and CAD extent
                        Logger.Instance.LogError(ex);
                        throw ex;
                    }
                }

                Logger.Instance.LogInfo($"Finished");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CoordinateSystem SetSourceProjection(string coordtype)
        {
            string[] values = coordtype
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToArray();

            switch (values[0])
            {
                case "1970":
                    switch (values[2])
                    {
                        case "K3":
                            return CoordinateSystem.BGS_1970_K3;
                        case "K5":
                            return CoordinateSystem.BGS_1970_K5;
                        case "K7":
                            return CoordinateSystem.BGS_1970_K7;
                        case "K9":
                            return CoordinateSystem.BGS_1970_K9;
                        default:
                            return CoordinateSystem.Unknown;
                    }
                default:
                    return CoordinateSystem.Unknown;
            }
        }



        /// <summary>
        /// За калкулиране на абсолютните координати на точки: <see cref="CADFileInfo.ReferencePoint"/>
        /// </summary>
        /// <param name="relativePoint"></param>
        /// <returns></returns>
        private Point CalculateAbsoluteCoordinatesFor(Point relativePoint)
            => this.FileInfo.ReferencePoint + relativePoint;
    }
}
