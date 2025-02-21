namespace SphrLibrary.Enums
{
    /// <summary>
    /// <see cref="SphrLibrary.Entities.FHIR.DocumentReference"/>のデータ種別を表します。
    /// </summary>
    [Flags()]
    public enum DocumentReferenceTypeEnum : UInt64
    {
        /// <summary>
        /// 未指定です。この値は未初期化を表します。初期化後に指定しないでください。
        /// </summary>
        None = 0,

        // OMH体重記録
        BodyWeight = 1,
        // OMH血圧記録
        BloodPressure = 2,
        // IEEE歩数記録
        PhysicalActivity = 4,
        // OMH体温記録 
        BodyTemperature = 8,
        // OMH酸素飽和度記録
        OxygenSaturation = 16,
        // 特定健診情報(HL7 CDA)
        SpecificHealthCheckupHL7CDA = 32,
        // 特定健診情報(FHIR)
        SpecificHealthCheckupFHIR = 64,
        // 調剤歴(マイナポータル)
        MynaPrescription = 128,
        // 調剤歴(お薬手帳QRコード)
        JahisPrescription = 256,
        // 予防接種歴
        MynaVaccination = 512,
    }
}
