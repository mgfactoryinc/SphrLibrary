namespace SphrLibrary.Enums
{
    /// <summary>
    /// <see cref="SphrLibrary.Entities.FHIR.Composition"/>の状態を表します。
    /// </summary>
    public enum CompositionStatusTypeEnum
    {
        //! 最終版 確定した情報を流通するためこの文言で固定とする。
        final,

        //! Compositionの仕様上は可能だが使用しない
        // registered,
        // partial,
        // preliminary,
        // amended,
        // corrected,
        // appended,
        // cancelled,
        // entered_in_error,
        // deprecated,
        // unknown,
    }
}
