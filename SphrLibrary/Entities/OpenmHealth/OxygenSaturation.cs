using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 酸素飽和度(SpO2)データのリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class OxygenSaturation : OmhEntityBase<OxygenSaturationBody>
    {
        public OxygenSaturation() : base() { }
    }
}
