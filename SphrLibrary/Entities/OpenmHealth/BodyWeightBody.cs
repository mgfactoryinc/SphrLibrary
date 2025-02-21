using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 体重データボディ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BodyWeightBody : OmhBodyEntityBase
    {
        public BodyWeightBody() : base() { }
    }
}
