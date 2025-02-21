using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 血圧データのリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BloodPressure : OmhEntityBase<BloodPressureBody>
    {
        public BloodPressure() : base() { }
    }
}
