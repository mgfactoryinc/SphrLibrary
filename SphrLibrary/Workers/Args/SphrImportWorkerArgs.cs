using SphrLibrary.Entities.SPHR;
using SphrLibrary.Workers.Base;

namespace SphrLibrary.Workers.Args
{
    /// <summary>
    /// 汎用モジュールが使用するインポートの引数クラスを表します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrImportWorkerArgs : SphrWorkerArgsBase
    {
        #region "Public Property"

        /// <summary>
        /// SPHRファイルパスを取得または設定します。
        /// </summary>
        /// <remarks>ファイルパスが指定された場合、ファイルパスが優先されます。</remarks>
        public string? SphrFilePath { get; set; } = null;

        /// <summary>
        /// .sphrバイナリデータを取得または設定します。
        /// </summary>
        /// <remarks>ファイルパスはnullまたは空白を指定してください。</remarks>
        public byte[]? SphrBinary { get; set; } = null;

        /// <summary>
        /// .sphrファイル名（バイナリ指定時）を取得または設定します。
        /// </summary>
        public string? FileName { get; set; } = null;

        #endregion

        #region "Constructor"

        /// <summary>
        /// 値を指定して、<see cref="SphrImportWorkerArgs"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="settings">汎用モジュール 設定クラス。</param>
        /// <param name="userId">対象ユーザーID。</param>
        /// <param name="profile">SPHRプロファイル情報。</param>
        public SphrImportWorkerArgs(SphrLibrarySettings? settings, string? userId, SphrProfile? profile) : base(settings, userId, profile) { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// 引数の各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public override bool IsValid()
        {
            bool result = base.IsValidBase();

            if (result) { 
                if (!String.IsNullOrWhiteSpace(SphrFilePath)) {
                    // ファイルパスでインポート
                } else if (this.SphrBinary != null && this.SphrBinary.Length > 0) { 
                    // バイナリでインポート
                } else {
                    result = false;
                }
            }

            return result;
        }

        #endregion


    }
}
