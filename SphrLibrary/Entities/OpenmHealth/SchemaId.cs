using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth SchemaId リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class SchemaId
    {
        [DataMember(Name = "namespace")]
        public string @namespace = string.Empty;
        [DataMember()]
        public string name = string.Empty;
        [DataMember()]
        public string version = string.Empty;

        public SchemaId() { }

        public SchemaId(string @namespace, string name, string version)
        {
            this.@namespace = @namespace;
            this.name = name;
            this.version = version;
        }
    }
}
