using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth ヘッダ部のリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class OmhHeader
    {
        [DataMember()]
        public string uuid = string.Empty;
        [DataMember()]
        public string source_creation_date_time = string.Empty;
        [DataMember()]
        public SchemaId schema_id = new SchemaId();
        [DataMember(EmitDefaultValue = false)]
        public string? modality = string.Empty;
        [DataMember(EmitDefaultValue = false)]
        public ValueUnit? acquisition_rate = new ValueUnit();
        [DataMember(EmitDefaultValue = false)]
        public ExternalDataSheet[]? external_data_sheets = [];

        public OmhHeader() { }
    }
}
