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
        SHEMI,
        /// <summary>
        /// зоза охранителните зони "А" и "Б" по Закона за устройството на Черноморското крайбрежие
        /// </summary>
        MORВRIAG,
        /// <summary>
        /// защитени територии и зони
        /// </summary>
        ZTZ,
        /// <summary>
        /// подземни проводи и съоръжения
        /// </summary>
        PODZEМNI
    }
}
