using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth ボディ部リソースの基本クラスを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public abstract class OmhBodyEntityBase
    {
        [DataMember()]
        public TimeFrame effective_time_frame = new TimeFrame();

        public OmhBodyEntityBase() { }
    }
}
