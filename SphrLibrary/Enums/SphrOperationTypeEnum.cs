namespace SphrLibrary.Enums
{
    /// <summary>
    /// 汎用モジュールの実行種別を表します。
    /// </summary>
    [Flags]
    public enum SphrOperationTypeEnum
    {
        None = 0,
        Export = 1,
        Import = 2,
        Extract = 4,
    }
}
