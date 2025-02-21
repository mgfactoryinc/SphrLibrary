using System.Runtime.Serialization;

namespace SphrLibrary.Entities.SPHR
{
    /// <summary>
    /// 汎用モジュール処理結果クラスを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class SphrResult
    {
        #region "Public Property"

        /// <summary>
        /// 処理結果コードを取得または設定します。
        /// </summary>
        [DataMember()]
        public string code { get; set; } = string.Empty;

        /// <summary>
        /// 処理結果詳細を取得または設定します。
        /// </summary>
        [DataMember()]
        public string detail { get; set; } = string.Empty;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrResult"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrResult() { }

        #endregion
    }
}
