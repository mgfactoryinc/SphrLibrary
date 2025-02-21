using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 歩数データボディ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class PhysicalActivityBody : OmhBodyEntityBase
    {
        [DataMember()]
        public string activity_name = string.Empty;
        //[DataMember()]
        //public EffectiveTimeFrame effective_time_frame = new EffectiveTimeFrame();
        [DataMember()]
        public ValueUnit distance = new ValueUnit();
        [DataMember()]
        public ValueUnit base_movement_quantity = new ValueUnit();

        public PhysicalActivityBody() : base() { }
    }
}
