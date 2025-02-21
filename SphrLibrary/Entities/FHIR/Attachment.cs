using System.Runtime.Serialization;

namespace SphrLibrary.Entities.FHIR
{
    /// <summary>
    /// FHIR Attachment リソースを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class Attachment
    {
        [DataMember()]
        public string contentType = string.Empty;
        [DataMember(EmitDefaultValue = false)]
        public string? language = null;
        [DataMember(EmitDefaultValue = false)]
        public string? data = null;
        [DataMember()]
        public string url = string.Empty;
        [DataMember()] // 値型はAOTでnull除外できないらしい
        public Int64? size = null;
        [DataMember(EmitDefaultValue = false)]
        public string? hash = null;
        [DataMember(EmitDefaultValue = false)]
        public string? title = null;
        [DataMember(EmitDefaultValue = false)]
        public string? creation = null;

        //public int height = 0;
        //public int width = 0;
        //public int frames = 0;
        //public decimal duration = 0;
        //public int pages = 0;

        public Attachment() { }
    }
}
