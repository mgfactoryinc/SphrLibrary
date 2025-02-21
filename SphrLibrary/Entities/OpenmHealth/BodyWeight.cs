using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 体重データのリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BodyWeight : OmhEntityBase<BodyWeightBody>
    {
        public BodyWeight() : base() { }
    }
}
