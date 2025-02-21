using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Organization リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Organization
    {
        //public Identifier? identifier = null;
        //public bool? active = null;
        //public CodableConcept? type = null;
        //public string? name = null;
        //public List<string>? alias = null;
        //public List<ContactPoint>? telecom = null;
        //public List<Address>? address = null;
        //public Reference<Organization>? partOf = null;
        //public List<BackboneElement>? contact = null;
        //public List<Reference<Endpoint>>? endpoint = null;

        public Organization() { }
    }
}
