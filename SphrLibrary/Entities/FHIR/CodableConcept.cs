using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR CodableConcept リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class CodableConcept
    {
        [DataMember()]
        public Coding[] coding = [];

        public CodableConcept() { }
    }
}
