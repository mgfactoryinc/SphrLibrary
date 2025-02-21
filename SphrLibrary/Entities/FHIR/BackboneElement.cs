using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR BackboneElement リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class BackboneElement
    {
        [DataMember()]
        public Attachment attachment = new Attachment();

        public BackboneElement() { }
    }
}
