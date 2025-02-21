using System.Runtime.Serialization;

namespace SphrLibrary.Entities.SPHR
{
    /// <summary>
    /// 汎用モジュール設定クラスを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class SphrLibrarySettings
    {
        #region "Public Property"

        /// <summary>
        /// サービスIDを取得または設定します。
        /// </summary>
        [DataMember()]
        public string ServiceId { get; set; } = string.Empty;

        /// <summary>
        /// サービス名を取得または設定します。
        /// </summary>
        [DataMember()]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// ストレージルートパスを取得または設定します。
        /// </summary>
        [DataMember()]
        public string StorageRootPath { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーIDの桁数を取得または設定します。
        /// </summary>
        /// <remarks>8桁以上を指定してください。</remarks>
        [DataMember()]
        public int UserIdDigits { get; set; } = -1;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrLibrarySettings"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrLibrarySettings() { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// 各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public bool IsValid() {
            return !string.IsNullOrWhiteSpace(ServiceId) && 
                !string.IsNullOrWhiteSpace(ServiceName) && 
                !string.IsNullOrWhiteSpace(StorageRootPath);
        }

        #endregion
    }
}