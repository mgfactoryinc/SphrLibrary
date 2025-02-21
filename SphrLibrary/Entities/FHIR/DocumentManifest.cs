using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR DocumentManifest リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class DocumentManifest : Fhir
    {
        [DataMember()]
        public string id = string.Empty;
        [DataMember()]
        public Reference[]? content = null; //Reference<int>[]? content = null;

        //public Identifier? masterIdentifier = null;
        //public Identifier? identifier = null;
        //public string status? = null;
        //public CodableConcept? type = null;
        //public Reference<Any>? subject = null;
        //public string? created = null;
        //public List<Reference<Any>>? author = null;
        //public List<Reference<Any>>? recipient = null;
        //public string? uri = null;
        //public string? description = null;
        //public List<BackboneElement>? related = null;

        public DocumentManifest() { }
    }
}
