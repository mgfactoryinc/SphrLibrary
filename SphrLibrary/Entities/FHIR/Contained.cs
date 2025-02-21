using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Contained リソースを表します。
    /// </summary>
    [DataContract()]
    public class Contained
    {
        [DataMember()]
        public string id = string.Empty;
        [DataMember()]
        public string name = string.Empty;
        [DataMember()]
        public string resourceType = string.Empty;

        public Contained() { }
    }
}
