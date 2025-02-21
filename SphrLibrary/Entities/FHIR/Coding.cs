using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Coding リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Coding
    {
        [DataMember()]
        public string code = string.Empty;
        [DataMember()]
        public string display = string.Empty;
        [DataMember()]
        public string system = string.Empty;

        public Coding() { }

        public Coding(string code, string display, string system)
        {
            this.code = code;
            this.display = display;
            this.system = system;
        }

        public string CodeSystem() {
            return string.Format("{0}/{1}", this.system, this.code);
        }
    }
}
