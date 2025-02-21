using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 体温データボディ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BodyTemperatureBody : OmhBodyEntityBase
    {
        public BodyTemperatureBody() : base() { }
    }
}
