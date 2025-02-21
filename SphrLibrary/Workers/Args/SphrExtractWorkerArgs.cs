using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Workers.Base;

namespace SphrLibrary.Workers.Args
{
    /// <summary>
    /// 汎用モジュールが使用するデータ抽出の引数クラスを表します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExtractWorkerArgs : SphrWorkerArgsBase
    {
        #region "Public Property"

        /// <summary>
        /// データ抽出対象項目種別を取得または設定します。
        /// </summary>
        public DocumentReferenceTypeEnum ExtractType { get; set; } = DocumentReferenceTypeEnum.None;

        #endregion

        #region "Constructor"

        /// <summary>
        /// 値を指定して、<see cref="SphrExtractWorkerArgs"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="settings">汎用モジュール 設定クラス。</param>
        /// <param name="userId">対象ユーザーID。</param>
        /// <param name="profile">SPHRプロファイル情報。</param>
        public SphrExtractWorkerArgs(SphrLibrarySettings? settings, string? userId, SphrProfile? profile) : base(settings, userId, profile) { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// 引数の各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public override bool IsValid()
        {
            return base.IsValidBase() && this.ExtractType != DocumentReferenceTypeEnum.None;
        }

        #endregion

    }
}
