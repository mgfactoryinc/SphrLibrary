using SphrLibrary.Entities.SPHR;
using SphrLibrary.Workers.Base;

namespace SphrLibrary.Workers.Args
{
    /// <summary>
    /// 汎用モジュールが使用するエクスポートの引数クラスを表します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExportWorkerArgs : SphrWorkerArgsBase
    {
        #region "Public Property"

        /// <summary>
        /// エクスポート日時を取得または設定します。
        /// </summary>
        public DateTime ExportDate { get; set; } = DateTime.MinValue;

        #endregion

        #region "Constructor"

        /// <summary>
        /// 値を指定して、<see cref="SphrExportWorkerArgs"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="settings">汎用モジュール 設定クラス。</param>
        /// <param name="userId">対象ユーザーID。</param>
        /// <param name="profile">SPHRプロファイル情報。</param>
        public SphrExportWorkerArgs(SphrLibrarySettings? settings, string? userId, SphrProfile? profile) : base(settings, userId, profile) { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// 引数の各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public override bool IsValid()
        {
            return base.IsValidBase() && this.ExportDate != DateTime.MinValue;
        }

        #endregion


    }
}
