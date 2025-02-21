using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 体温データのリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BodyTemperature : OmhEntityBase<BodyTemperatureBody>
    {
        public BodyTemperature() : base() { }
    }
}
