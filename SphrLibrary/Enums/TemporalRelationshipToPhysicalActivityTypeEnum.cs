namespace SphrLibrary.Enums
{
    /// <summary>
    /// 睡眠との時間的関係 を表します。
    /// </summary>
    public enum TemporalRelationshipToPhysicalActivityTypeEnum
    {
        //! 安静時
        at_rest,
        //! 活動時
        active,
        //! 運動前
        before_exercise,
        //! 運動後
        after_exercise,
        //! 運動中
        during_exercise,
    }
}
