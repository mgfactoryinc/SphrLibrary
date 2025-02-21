using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth ValueUnit リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class ValueUnit
    {
        [DataMember()]
        public double value = 0;

        [DataMember()]
        public string unit = string.Empty;

        public ValueUnit() { }
    }
}
