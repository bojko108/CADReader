namespace CAD.Nomenclature
{
    /// <summary>
    /// Графичната информация се съдържа в графични блокове, описващи различните слоеве
    /// </summary>
    public enum CADLayerType
    {
        /// <summary>
        /// кадастрални данни
        /// </summary>
        CADASTER,
        /// <summary>
        /// лесоустройствени проекти
        /// </summary>
        LESO,
        /// <summary>
        /// почвена категория
        /// </summary>
        POCHKATEG,
        /// <summary>
        /// регулационен план
        /// </summary>
        REGPLAN,
        /// <summary>
        /// описание на схеми на самостоятелни обекти в сгради
        /// </summary>
        SHEMI
    }
}
