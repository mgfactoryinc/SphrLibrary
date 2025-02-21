namespace SphrLibrary.Enums
{
    /// <summary>
    /// 測定結果を取得したモダリティ（例：測定機器か自己報告か）。
    /// </summary>
    public enum ModalityTypeEnum
    {
        //! 測定機器からの入力
        sensed,
        //! 手入力
        self_reported,
    }
}
