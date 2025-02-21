namespace SphrLibrary.Enums
{
    /// <summary>
    /// <see cref="SphrLibrary.Entities.FHIR.DocumentManifest"/>、<see cref="SphrLibrary.Entities.FHIR.DocumentReference"/>の状態を表します。
    /// </summary>
    public enum DocumentStatusTypeEnum
    {
        //! 現状版 取得依頼時点の情報を流通するためこの文言で固定とする。
        current,

        //! DocumentManifestの仕様上は可能だが使用しない
        // superseded,
        // entered_in_error,
    }
}
