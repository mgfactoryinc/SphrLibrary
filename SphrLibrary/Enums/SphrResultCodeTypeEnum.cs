namespace SphrLibrary.Enums
{
    /// <summary>
    /// 汎用モジュール処理結果種別を表します。
    /// </summary>
    public enum SphrResultCodeTypeEnum : byte
    {
        None = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Debug = 255,
    }
}
