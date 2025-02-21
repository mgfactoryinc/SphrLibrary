using SphrLibrary.Entities.SPHR;

namespace SphrLibrary.Workers.Base
{
    /// <summary>
    /// 汎用モジュールが使用する各機能の引数の基本クラスを表します。
    /// </summary>
    internal abstract class SphrWorkerArgsBase
    {
        #region "Public Property"

        /// <summary>
        /// 汎用モジュール 設定クラスを取得または設定します。
        /// </summary>
        public SphrLibrarySettings? Settings { get; set; } = null;

        /// <summary>
        /// 対象ユーザーIDを取得または設定します。
        /// </summary>
        public string? UserId { get; set; } = null;

        /// <summary>
        /// PHRプロファイル情報を取得または設定します。
        /// </summary>
        public SphrProfile? Profile { get; set; } = null;

        #endregion

        #region "Constructor"

        /// <summary>
        /// デフォルトコンストラクタは使用できません。
        /// </summary>
        private SphrWorkerArgsBase() { }

        /// <summary>
        /// 値を指定して、<see cref="SphrWorkerArgsBase"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userId"></param>
        /// <param name="profile"></param>
        protected SphrWorkerArgsBase(SphrLibrarySettings? settings, string? userId, SphrProfile? profile)
        {
            this.Settings = settings;
            this.UserId = userId;
            this.Profile = profile;
        }

        #endregion

        #region "MustOverride Method"

        /// <summary>
        /// 引数の各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public abstract bool IsValid();

        #endregion

        #region "Public Method"

        /// <summary>
        /// 基本クラスの各プロパティの有効性を検証します。
        /// </summary>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public bool IsValidBase()
        {
            return this.Settings != null && this.Settings.IsValid() && !string.IsNullOrWhiteSpace(this.UserId);
        }

        #endregion
    }
}
