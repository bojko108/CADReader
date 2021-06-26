using BojkoSoft.Transformations;
using RBush;

namespace CAD.Geometry
{
    /// <summary>
    /// Представлява правоъгълник
    /// </summary>
    public class Extent : IExtent
    {
        /// <summary>
        /// минимална стойност за Northing (x)
        /// </summary>
        public double MinN { get; set; }
        /// <summary>
        /// минимална стойност за Easting (y)
        /// </summary>
        public double MinE { get; set; }
        /// <summary>
        /// максимална стойност за Northing (x)
        /// </summary>
        public double MaxN { get; set; }
        /// <summary>
        /// максимална стойност за Easting (y)
        /// </summary>
        public double MaxE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public double Height { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// Създава нов обхват с посочените параметри
        /// </summary>
        /// <param name="minN">минимална стойност за Northing (x)</param>
        /// <param name="minE">минимална стойност за Easting (y)</param>
        /// <param name="maxN">максимална стойност за Northing (x)</param>
        /// <param name="maxE">максимална стойност за Easting (y)</param>
        public Extent(double minN, double minE, double maxN, double maxE)
        {
            this.MinN = minN;
            this.MinE = minE;
            this.MaxN = maxN;
            this.MaxE = maxE;

            this.Expand(0);
        }

        /// <summary>
        /// Converts this object to <see cref="Envelope"/>
        /// </summary>
        /// <returns></returns>
        internal Envelope ToEnvelope()
            => new Envelope(this.MinE, this.MinN, this.MaxE, this.MaxN);

        /// <summary>
        /// Converts this object to <see cref="Extent"/>
        /// </summary>
        /// <returns></returns>
        internal static Extent FromEnvelope(Envelope envelope)
            => new Extent(envelope.MinY, envelope.MinX, envelope.MaxY, envelope.MaxX);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meters"></param>
        public void Expand(double meters)
        {
            this.MaxN += meters;
            this.MaxE += meters;
            this.MinN -= meters;
            this.MinE -= meters;

            this.Width = this.MaxE - this.MinE;
            this.Height = this.MaxN - this.MinN;
            this.IsEmpty = this.Width <= 0 && this.Height <= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IExtent Clone()
            => new Extent(this.MinN, this.MinE, this.MaxN, this.MaxE);
    }
}
