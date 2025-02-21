using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth TimeFrame リソースを表します。
    /// </summary>
    [DataContract()]
    public class TimeFrame
    {
        [DataMember(EmitDefaultValue = false)]
        public string? date_time = null; 

        [DataMember(EmitDefaultValue = false)]
        public TimeInterval? time_interval = null;

        public TimeFrame() { }
    }
}
