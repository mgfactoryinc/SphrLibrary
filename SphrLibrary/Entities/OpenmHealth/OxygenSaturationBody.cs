using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 酸素飽和度(SpO2)データボディ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class OxygenSaturationBody : OmhBodyEntityBase
    {
        public OxygenSaturationBody() : base() { }
    }
}
