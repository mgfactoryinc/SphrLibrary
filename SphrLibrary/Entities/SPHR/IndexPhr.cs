using SphrLibrary.Entities.FHIR;
using System.Runtime.Serialization;

namespace SphrLibrary.Entities.SPHR
{
    /// <summary>
    /// SPHRインデックス情報を表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class IndexPhr
    {
        #region "Public Property"

        /// <summary>
        /// FHIR Composition リソースプロファイルを取得または設定します。
        /// </summary>
        [DataMember()]
        public Composition Composition { get; set; } = new Composition();

        /// <summary>
        /// FHIR DocumentManifest リソースプロファイルを取得または設定します。
        /// </summary>
        [DataMember()]
        public DocumentManifest DocumentManifest { get; set; } = new DocumentManifest();

        /// <summary>
        /// FHIR DocumentReference リソースプロファイルを取得または設定します。
        /// </summary>
        [DataMember()]
        public List<DocumentReference> DocumentReferences { get; set; } = new List<DocumentReference>();

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="IndexPhr"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public IndexPhr() { }

        #endregion
    }
}
