using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 血圧データボディ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BloodPressureBody : OmhBodyEntityBase
    {
        [DataMember()]
        public ValueUnit systolic_blood_pressure = new ValueUnit();
        [DataMember()]
        public ValueUnit diastolic_blood_pressure = new ValueUnit();

        [DataMember(EmitDefaultValue = false)]
        public string? body_posture = null; //BodyPostureTypeEnum
        [DataMember(EmitDefaultValue = false)]
        public string? measurement_location = null; //BloodPressureMeasurementLocationTypeEnum
        [DataMember(EmitDefaultValue = false)]
        public string? temporal_relationship_to_physical_activity = null; //TemporalRelationshipToPhysicalActivityTypeEnum
        [DataMember(EmitDefaultValue = false)]
        public string? descriptive_statistic = null;

        public BloodPressureBody() : base() { }
    }
}
