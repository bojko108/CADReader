using CAD.Entity;
using CAD.Geometry;
using CAD.Nomenclature;
using RBush;
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

        private readonly Stream cadFile;

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="filePath">път до файла</param>
        public CADFile(string filePath)
        {
            if (File.Exists(filePath) == false)
                throw new ArgumentException($"File does not exist or is not accessible: {filePath}");

            this.cadFile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            this.ReadFile();
        }

        /// <summary>
        /// Създава нов обект за четене на CAD файл v4
        /// </summary>
        /// <param name="fileContents">съдържание на CAD файла</param>
        public CADFile(byte[] fileContents)
        {
            this.cadFile = new MemoryStream(fileContents);
            this.ReadFile();
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
        /// четене на CAD файла
        /// </summary>
        private void ReadFile()
        {
            try
            {
                this.FileInfo = new CADFileInfo();
                this.Layers = new List<CADLayer>();

                using (StreamReader sr = new StreamReader(this.cadFile, Encoding.UTF8))
                {
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
                                            throw new ArgumentException("Only CAD files v4 are supported");
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
                                        this.FileInfo.Window = new Envelope(
                                            double.Parse(values[0]),
                                            double.Parse(values[1]),
                                            double.Parse(values[2]),
                                            double.Parse(values[3]));
                                        continue;
                                    case CADConstants.COORDTYPE:
                                        this.FileInfo.Coordtype = line.Substring(separatorIndex + 1);
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

                        #endregion

                        #region Read GRAPHICS section

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
                                        geoPoint.SetGeometry(this.CalculateCoordinatesFor(geoPoint.Geometry as Point));

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

                                                Point point = new Point(pointData
                                                   .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                                                point = this.CalculateCoordinatesFor(point);

                                                vertices.Add(point);
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
                                        interiorPoint = this.CalculateCoordinatesFor(interiorPoint);
                                        (contour as CADContour).InteriorPoint = this.CalculateCoordinatesFor(interiorPoint);

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
                                        symbol.SetGeometry(this.CalculateCoordinatesFor(symbol.Geometry as Point));

                                        layer.AddEntity(symbol);
                                        continue;

                                    // TEXT entity
                                    case CADConstants.T:
                                        values = line
                                             .Substring(separatorIndex + 1)
                                             .Split(CADConstants.SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                        CADText text = new CADText(values);
                                        text.SetGeometry(this.CalculateCoordinatesFor(text.Geometry as Point));
                                        line = sr.ReadLine();
                                        if (string.IsNullOrEmpty(line))
                                            continue;
                                        line.Trim();
                                        text.SetText(line);

                                        layer.AddEntity(text);
                                        continue;
                                }
                            }

                            this.Layers.Add(layer);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// За калкулиране на абсолютните координати на точки: <see cref="CADFileInfo.ReferencePoint"/>
        /// </summary>
        /// <param name="relativePoint"></param>
        /// <returns></returns>
        private Point CalculateCoordinatesFor(Point relativePoint)
        {
            // add projection...

            return this.FileInfo.ReferencePoint + relativePoint;
        }
    }
}
