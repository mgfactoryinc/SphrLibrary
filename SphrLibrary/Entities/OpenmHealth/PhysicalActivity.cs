using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth 歩数データのリソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class PhysicalActivity : OmhEntityBase<PhysicalActivityBody>
    {
        public PhysicalActivity() : base() { }

    }
}
