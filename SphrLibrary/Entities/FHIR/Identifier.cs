using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Identifier リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Identifier
    {
        //public string? use = null;
        //public CodableConcept? type = null;
        //public string? system = null;
        //public string? value = null;
        //public Period? period = null;
        //public Reference<Organization>? assigner = null;

        public Identifier() { }
    }
}
