using RBush;

namespace CAD.Geometry
{
    /// <summary>
    /// Представлява правоъгълник
    /// </summary>
    public class Extent
    {
        /// <summary>
        /// минимална стойност за Northing (x)
        /// </summary>
        public double MinN { get; private set; }
        /// <summary>
        /// минимална стойност за Easting (y)
        /// </summary>
        public double MinE { get; private set; }
        /// <summary>
        /// максимална стойност за Northing (x)
        /// </summary>
        public double MaxN { get; private set; }
        /// <summary>
        /// максимална стойност за Easting (y)
        /// </summary>
        public double MaxE { get; private set; }

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
        }

        /// <summary>
        /// Converts this object to <see cref="Envelope"/>
        /// </summary>
        /// <returns></returns>
        internal Envelope ToEnvelope()
            => new Envelope(this.MinE, this.MinN, this.MaxE, this.MaxN);
    }
}
