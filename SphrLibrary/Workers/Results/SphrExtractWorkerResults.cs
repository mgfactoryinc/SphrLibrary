using SphrLibrary.Entities.SPHR;
using SphrLibrary.Workers.Base;

namespace SphrLibrary.Workers.Results
{
    /// <summary>
    /// 汎用モジュールが使用するデータ抽出の戻り値クラスを表します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExtractWorkerResults : SphrWorkerResultsBase
    {
        #region "Public Property"

        /// <summary>
        /// SPHRプロファイル情報を取得または設定します。
        /// </summary>
        public SphrProfile? Profile { get; set; } = null;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrExtractWorkerResults"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrExtractWorkerResults() { }

        #endregion
    }
}
