using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR リソースの基本クラスを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public abstract class Fhir
    {
        [DataMember()]
        public string status = string.Empty;
        [DataMember()]
        public string resourceType = string.Empty;

        public Fhir() { }
    }
}
