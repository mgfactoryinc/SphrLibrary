using SphrLibrary.Workers.Base;

namespace SphrLibrary.Workers.Results
{
    /// <summary>
    /// 汎用モジュールが使用するエクスポートの戻り値クラスを表します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExportWorkerResults : SphrWorkerResultsBase
    {
        #region "Public Property"

        /// <summary>
        /// エクスポート先のファイルパスを取得または設定します。
        /// </summary>
        public string ExportFilePath { get; set; } = string.Empty;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrExportWorkerResults"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrExportWorkerResults() { }

        #endregion
    }
}
