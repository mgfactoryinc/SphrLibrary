using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Reference リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Reference
    {
        [DataMember()]
        public string reference = string.Empty;

        //public string type = string.Empty;
        //public Identifier identifier = string.Empty;
        //public string display = string.Empty;

        public Reference() { }
    
    }
}
