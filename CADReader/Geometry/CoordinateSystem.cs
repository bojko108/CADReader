namespace CAD.Geometry
{
    /// <summary>
    /// Available projections
    /// </summary>
    public enum CoordinateSystem
    {
        /// <summary>
        /// Projection is unknown
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Ellipsoid WGS 1984
        /// </summary>
        WGS84 = 7030,
        /// <summary>
        /// Normal Sphere (R=6378137)
        /// </summary>
        SPHERE = 0,

        /// <summary>
        /// Geographic coordiantes on WGS84 Ellipsoid
        /// </summary>
        WGS84_GEOGRAPHIC = 4326,
        /// <summary>
        /// BGS Sofia. Local projection based on BGS 1950
        /// </summary>
        BGS_SOFIA = 108501,
        /// <summary>
        /// Gauss projection based on Hayford ellipsoid
        /// </summary>
        BGS_1930_24 = 1083024,
        /// <summary>
        /// Gauss projection based on Hayford ellipsoid
        /// </summary>
        BGS_1930_27 = 1083027,
        /// <summary>
        /// Gauss projection based on Krassovsky ellipsoid
        /// </summary>
        BGS_1950_3_24 = 10850324,
        /// <summary>
        /// Gauss projection based on Krassovsky ellipsoid
        /// </summary>
        BGS_1950_3_27 = 10850327,
        /// <summary>
        /// Gauss projection based on Krassovsky ellipsoid
        /// </summary>
        BGS_1950_6_21 = 10850621,
        /// <summary>
        /// Gauss projection based on Krassovsky ellipsoid
        /// </summary>
        BGS_1950_6_27 = 10850627,
        /// <summary>
        /// ~ Northewest Bulgaria
        /// </summary>
        BGS_1970_K3 = 108703,
        /// <summary>
        /// ~ Southeast Bulgaria
        /// </summary>
        BGS_1970_K5 = 108705,
        /// <summary>
        /// ~ Northeast Bulgaria
        /// </summary>
        BGS_1970_K7 = 108707,
        /// <summary>
        /// ~ Southwest Bulgaria
        /// </summary>
        BGS_1970_K9 = 108709,
        /// <summary>
        /// Lambert Conformal Conic with 2SP used by Cadastral Agency
        /// </summary>
        BGS_2005_KK = 7801,
        /// <summary>
        /// UTM zone 34 North
        /// </summary>
        UTM34N = 32634,
        /// <summary>
        /// UTM zone 35 North
        /// </summary>
        UTM35N = 32635
    }
}
