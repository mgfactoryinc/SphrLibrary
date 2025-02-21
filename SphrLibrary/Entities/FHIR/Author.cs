using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Author リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Author
    {
        [DataMember()]
        public string reference = string.Empty;

        public Author() { }
    }
}
