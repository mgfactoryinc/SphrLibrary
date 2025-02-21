using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth TimeInterval リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class TimeInterval
    {
        [DataMember()]
        public string start_date_time = string.Empty;
        [DataMember()]
        public string end_date_time = string.Empty;

        public TimeInterval() { }
    }
}
