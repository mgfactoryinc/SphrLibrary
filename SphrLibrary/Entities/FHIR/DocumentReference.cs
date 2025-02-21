using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR DocumentReference リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class DocumentReference : Fhir
    {
        [DataMember()]
        public string id = string.Empty;
        [DataMember()]
        public CodableConcept type = new CodableConcept();
        [DataMember()]
        public BackboneElement[] content = [];

        // https://hl7.org/fhir/R5/documentreference.html
        //public Identifier? identifier = null;
        //public string version = string.Empty;
        //public Reference? basedOn = null;
        //public string status = string.Empty;
        //public string docStatus = string.Empty;
        //public List<CodableConcept>? modality = null;
        //public List<CodableConcept>? category  = null;
        //public Reference<Any>? subject = null;
        //public List<Reference<Any>>? context = null;
        //public List<CodableConcept>? @event = null;
        //public CodableConcept<BodyStructure>? bodySite = null;
        //public CodableConcept? facilityType = null;
        //public CodableConcept? practiceSetting = null;
        //public Period? period = null;
        //public string date = string.Empty;
        //public Reference<Any>? description = string.Empty;
        //public List<Attester>? attester = null;
        //public Reference<Organization>? custodian = null;
        //public List<RelatesTo>? relatesTo = null;
        //public string description = string.Empty;
        //public List<CodableConcept>? securityLabel = null;

        public DocumentReference() { }
    }
}
