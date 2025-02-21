using SphrLibrary.Entities.SPHR;

namespace SphrLibrary.Workers.Base
{
    /// <summary>
    /// 汎用モジュールが使用する各機能の戻り値の基本クラスを表します。
    /// </summary>
    internal abstract class SphrWorkerResultsBase
    {
        #region "Public Property"

        /// <summary>
        /// 処理結果を取得または設定します。
        /// </summary>
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 処理結果詳細クラスを取得または設定します。
        /// </summary>
        public List<SphrResult>? Results { get; set; } = null;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrWorkerArgsBase"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        protected SphrWorkerResultsBase() { }

        #endregion


    }
}
