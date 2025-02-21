using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Composition リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Composition : Fhir
    {
        [DataMember()]
        public Contained[] contained = [];
        [DataMember()]
        public Author[] author = [];
        [DataMember()]
        public string date = string.Empty;
        [DataMember()]
        public string title = string.Empty;
        [DataMember()]
        public CodableConcept type = new CodableConcept();

        public Composition() { }
    }
}
