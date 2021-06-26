using BojkoSoft.Transformations;
using CAD.Geometry;

namespace CAD.Internals
{
    /// <summary>
    /// Този клас се използва за трансформация на координатите в CAD файла
    /// </summary>
    internal sealed class TransformationUtils
    {
        /// <summary>
        /// Използва се за трансформация на координатите в CAD файла
        /// </summary>
        public static TransformationUtils Instance => _instance;
        /// <summary>
        /// Координатна система на данните в CAD файла
        /// </summary>
        public CoordinateSystem SourceProjection { get; set; }
        /// <summary>
        /// Целева координатна система
        /// </summary>
        public CoordinateSystem TargetProjection { get; set; }
        /// <summary>
        /// Проверява дали е необходима трансформация - тя не е нужна 
        /// ако координатните системи съвпадат или не са зададени
        /// </summary>
        public bool NeedsTransformation
        {
            get
            {
                return this.SourceProjection != CoordinateSystem.Unknown
                    && this.TargetProjection != CoordinateSystem.Unknown
                    && this.SourceProjection != this.TargetProjection;
            }
        }

        private static readonly TransformationUtils _instance = new TransformationUtils();
        private readonly Transformations tr;
        private double[] parameters;

        /// <summary>
        /// Създава нов клас за трансформации на координати
        /// </summary>
        private TransformationUtils()
        {
            this.tr = new Transformations();
            this.parameters = null;

            this.SourceProjection = CoordinateSystem.Unknown;
            this.TargetProjection = CoordinateSystem.Unknown;
        }

        /// <summary>
        /// Използва се за калкулиране на трансформационни параметри за афинна трансформация. 
        /// Подаденият обхват се увеличава с 20км и се използва за търсене на контролни точки.
        /// </summary>
        /// <param name="cadFileExtent">Обхват на данните в CAD файла</param>
        public void CalculateTransformationParameters(Extent cadFileExtent)
        {
            if (this.NeedsTransformation)
            {
                int source, target;

                #region Only old BGS coordiante systems have control points

                if (this.SourceProjection == CoordinateSystem.UTM34N
                    || this.SourceProjection == CoordinateSystem.UTM35N
                    || this.SourceProjection == CoordinateSystem.WGS84_GEOGRAPHIC)
                    source = (int)CoordinateSystem.BGS_2005_KK;
                else
                    source = (int)this.SourceProjection;

                if (this.TargetProjection == CoordinateSystem.UTM34N
                    || this.TargetProjection == CoordinateSystem.UTM35N
                    || this.TargetProjection == CoordinateSystem.WGS84_GEOGRAPHIC)
                    target = (int)CoordinateSystem.BGS_2005_KK;
                else
                    target = (int)this.TargetProjection;

                #endregion

                Extent expanded = (Extent)cadFileExtent.Clone();
                if (expanded.IsEmpty || expanded.Height < 20000 || expanded.Width < 20000)
                    expanded.Expand(20000);
                this.parameters = this.tr.CalculateAffineTransformationParameters(expanded, source, target);
            }
        }

        /// <summary>
        /// Трансформира точка от <see cref="TransformationUtils.SourceProjection"/> към 
        /// <see cref="TransformationUtils.TargetProjection"/>.
        /// </summary>
        /// <param name="sourcePoint"></param>
        /// <returns></returns>
        public Point TransformPoint(Point sourcePoint)
        {
            if (this.NeedsTransformation)
            {
                IPoint sourcePnt = sourcePoint;
                IPoint outputPoint = null;

                switch (this.SourceProjection)
                {
                    case CoordinateSystem.BGS_SOFIA:
                    case CoordinateSystem.BGS_1970_K3:
                    case CoordinateSystem.BGS_1970_K5:
                    case CoordinateSystem.BGS_1970_K7:
                    case CoordinateSystem.BGS_1970_K9:
                        {
                            switch (this.TargetProjection)
                            {
                                case CoordinateSystem.BGS_SOFIA:
                                case CoordinateSystem.BGS_1970_K3:
                                case CoordinateSystem.BGS_1970_K5:
                                case CoordinateSystem.BGS_1970_K7:
                                case CoordinateSystem.BGS_1970_K9:
                                case CoordinateSystem.BGS_2005_KK:
                                    {
                                        outputPoint = this.tr.TransformBGSCoordinates(sourcePnt, this.parameters, (int)this.SourceProjection, (int)this.TargetProjection);
                                        break;
                                    }
                                case CoordinateSystem.UTM34N:
                                case CoordinateSystem.UTM35N:
                                    {
                                        outputPoint = this.tr.TransformBGSCoordinates(sourcePnt, this.parameters, (int)this.SourceProjection, (int)CoordinateSystem.BGS_2005_KK);
                                        outputPoint = this.tr.TransformLambertToGeographic(outputPoint);
                                        outputPoint = this.tr.TransformGeographicToUTM(outputPoint, (int)this.TargetProjection, (int)CoordinateSystem.WGS84);
                                        break;
                                    }
                                case CoordinateSystem.WGS84_GEOGRAPHIC:
                                    {
                                        outputPoint = this.tr.TransformBGSCoordinates(sourcePnt, this.parameters, (int)this.SourceProjection, (int)CoordinateSystem.BGS_2005_KK);
                                        outputPoint = this.tr.TransformLambertToGeographic(outputPoint);
                                        break;
                                    }
                            }
                            break;
                        }
                    case CoordinateSystem.BGS_2005_KK:
                        {
                            switch (this.TargetProjection)
                            {
                                case CoordinateSystem.BGS_SOFIA:
                                case CoordinateSystem.BGS_1970_K3:
                                case CoordinateSystem.BGS_1970_K5:
                                case CoordinateSystem.BGS_1970_K7:
                                case CoordinateSystem.BGS_1970_K9:
                                    {
                                        outputPoint = this.tr.TransformBGSCoordinates(sourcePnt, this.parameters, (int)this.SourceProjection, (int)this.TargetProjection);
                                        break;
                                    }
                                case CoordinateSystem.UTM34N:
                                case CoordinateSystem.UTM35N:
                                    {
                                        outputPoint = this.tr.TransformLambertToGeographic(sourcePnt);
                                        outputPoint = this.tr.TransformGeographicToUTM(outputPoint, (int)this.TargetProjection, (int)CoordinateSystem.WGS84);
                                        break;
                                    }
                                case CoordinateSystem.WGS84_GEOGRAPHIC:
                                    {
                                        outputPoint = this.tr.TransformLambertToGeographic(sourcePnt);
                                        break;
                                    }
                            }
                            break;
                        }
                    case CoordinateSystem.UTM34N:
                    case CoordinateSystem.UTM35N:
                        {
                            switch (TargetProjection)
                            {
                                case CoordinateSystem.BGS_SOFIA:
                                case CoordinateSystem.BGS_1970_K3:
                                case CoordinateSystem.BGS_1970_K5:
                                case CoordinateSystem.BGS_1970_K7:
                                case CoordinateSystem.BGS_1970_K9:
                                    {
                                        outputPoint = this.tr.TransformUTMToGeographic(sourcePnt, (int)this.SourceProjection, (int)CoordinateSystem.WGS84);
                                        outputPoint = this.tr.TransformGeographicToLambert(outputPoint);
                                        outputPoint = this.tr.TransformBGSCoordinates(outputPoint, this.parameters, (int)CoordinateSystem.BGS_2005_KK, (int)this.TargetProjection);
                                        break;
                                    }
                                case CoordinateSystem.BGS_2005_KK:
                                    {
                                        outputPoint = this.tr.TransformUTMToGeographic(sourcePnt, (int)this.SourceProjection, (int)CoordinateSystem.WGS84);
                                        outputPoint = this.tr.TransformGeographicToLambert(outputPoint);
                                        break;
                                    }
                                case CoordinateSystem.UTM34N:
                                case CoordinateSystem.UTM35N:
                                    {
                                        outputPoint = this.tr.TransformUTMToGeographic(sourcePnt, (int)this.SourceProjection, (int)CoordinateSystem.WGS84);
                                        outputPoint = this.tr.TransformGeographicToUTM(outputPoint, (int)this.TargetProjection, (int)CoordinateSystem.WGS84);
                                        break;
                                    }
                                case CoordinateSystem.WGS84_GEOGRAPHIC:
                                    {
                                        outputPoint = this.tr.TransformUTMToGeographic(sourcePnt, (int)this.SourceProjection, (int)CoordinateSystem.WGS84);
                                        break;
                                    }
                            }
                            break;
                        }
                    case CoordinateSystem.WGS84_GEOGRAPHIC:
                        {
                            switch (this.TargetProjection)
                            {
                                case CoordinateSystem.BGS_SOFIA:
                                case CoordinateSystem.BGS_1970_K3:
                                case CoordinateSystem.BGS_1970_K5:
                                case CoordinateSystem.BGS_1970_K7:
                                case CoordinateSystem.BGS_1970_K9:
                                    {
                                        outputPoint = this.tr.TransformGeographicToLambert(sourcePnt);
                                        outputPoint = this.tr.TransformBGSCoordinates(outputPoint, this.parameters, (int)CoordinateSystem.BGS_2005_KK, (int)this.TargetProjection);
                                        break;
                                    }
                                case CoordinateSystem.BGS_2005_KK:
                                    {
                                        outputPoint = this.tr.TransformGeographicToLambert(sourcePnt);
                                        break;
                                    }
                                case CoordinateSystem.UTM34N:
                                case CoordinateSystem.UTM35N:
                                    {
                                        outputPoint = this.tr.TransformGeographicToUTM(sourcePnt, (int)this.TargetProjection, (int)CoordinateSystem.WGS84);
                                        break;
                                    }
                            }
                            break;
                        }
                }

                return new Point(outputPoint.N, outputPoint.E, outputPoint.Z)
                {
                    GUID = sourcePoint.GUID,
                    PointInfo = sourcePoint.PointInfo,
                    CoordinateSystem = this.TargetProjection
                };
            }
            else
            {
                sourcePoint.CoordinateSystem = this.TargetProjection;
                return sourcePoint;
            }
        }
    }
}
