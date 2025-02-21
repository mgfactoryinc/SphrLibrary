using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth ExternalDataSheet リソースを表します。
    /// </summary>
    [DataContract()]
    public class ExternalDataSheet
    {
        [DataMember()]
        public string datasheet_type = string.Empty;
        [DataMember()]
        public string datasheet_reference = string.Empty;

        public ExternalDataSheet() { }
    }
}
