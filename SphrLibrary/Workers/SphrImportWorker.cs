using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Helpers;
using SphrLibrary.Workers.Args;
using SphrLibrary.Workers.Base;
using SphrLibrary.Workers.Results;

namespace SphrLibrary.Workers
{
    /// <summary>
    /// 汎用モジュールが使用するインポートに関する機能を提供します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrImportWorker : SphrWorkerBase<SphrImportWorkerArgs, SphrImportWorkerResults>
    {
        #region "Constructor"

        /// <summary>
        /// <see cref="SphrImportWorker"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrImportWorker() : base(SphrOperationTypeEnum.Import) { }

        #endregion

        #region "Private Method"

        private (string serviceId, string exportDate) GetSegments(string dirName)
        {
            (string serviceId, string exportDate) result = ("", "");

            if (!string.IsNullOrWhiteSpace(dirName)) { 
                string[] segments = dirName.Split("_");
                result.serviceId = segments[0];
                result.exportDate = segments.Length >= 2 ? segments[1] : string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 作業フォルダに展開したファイルをストレージにインポートします。
        /// </summary>
        /// <param name="settings">汎用モジュール設定クラス。</param>
        /// <param name="userId">対象ユーザーID。</param>
        /// <param name="tempDir">展開済み作業フォルダパス。</param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        private bool ImportStorage(SphrLibrarySettings settings, string userId, string tempDir)
        {
            bool result = true;

            try {
                string userDir = FileIOHelper.SourcePath(settings.StorageRootPath, userId);

                foreach (string importSource in Directory.GetDirectories(SphrHelper.GetImportRoot(tempDir), "*", SearchOption.TopDirectoryOnly)) {
                    string newDirName = Path.GetFileName(importSource);
                    (string serviceId, string exportDate) newSegments = this.GetSegments(newDirName);
                    bool isImport = true;
                    
                    // 移動させる前にインポート済フォルダのチェック
                    Directory.GetDirectories(userDir, "*", SearchOption.TopDirectoryOnly).ToList().ForEach(x => {
                        string oldDirName = Path.GetFileName(x);
                        (string serviceId, string exportDate) oldSegments = this.GetSegments(oldDirName);

                        if (oldDirName.CompareTo(newDirName) == 0) {
                            // 同名は後勝ち（無条件削除）
                            FileIOHelper.DeleteDirectory(x);
                        } else if (oldSegments.serviceId.CompareTo(newSegments.serviceId) == 0) {
                            if (string.IsNullOrWhiteSpace(oldSegments.exportDate)) {
                                // インポート済みの日時が不明な場合は、今回のを採用
                                FileIOHelper.DeleteDirectory(x);
                            } else if (string.IsNullOrWhiteSpace(newSegments.exportDate)) {
                                // 今回インポートするのが日時不明な場合、インポートしない（インポート済みを採用）
                                isImport = false;
                            } else if (Convert.ToDecimal(oldSegments.exportDate) < Convert.ToDecimal(newSegments.exportDate)) {
                                // サービスID一致はエクスポート日時が新しい方を採用（インポート済が古い場合のみ削除）
                                FileIOHelper.DeleteDirectory(x);
                            } else {
                                // 今回インポートされたものの方が古いのでインポートしない
                                isImport = false;
                            }
                        } else { 
                            // 競合するフォルダなし
                        }
                    });

                    if (isImport) {
                        // temp -> ユーザーフォルダへ移動
                        if (SphrHelper.Move(importSource, FileIOHelper.BuildPath([userDir, newDirName], true, false))) { 
                            // OK。次へ
                        } else {
                            // NG。中途半端にインポートさせないため、1つでも失敗したらエラー扱い
                            result = false;
                            throw new InvalidOperationException(string.Format("ユーザーフォルダへの移動に失敗しました。:{0}", importSource));
                        }
                    } else {
                        // インポートしない
                        FileIOHelper.DeleteDirectory(importSource);
                        LogHelper.Write(string.Format("エクスポート日時が新しいフォルダが存在するためインポートされません。: {0}", newDirName));
                    }
                }
            } catch {
                result = false;
                throw;
            }

            return result;
        }

        #endregion

        #region "Public Method"

        /// <summary>
        /// インポートを実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public override SphrImportWorkerResults Execute(SphrImportWorkerArgs args)
        {
            SphrImportWorkerResults result = new SphrImportWorkerResults();

            try {
                if (args != null && args.IsValid()) {
                    LogHelper.Write(string.Format("インポートを開始します。: {0}", args.UserId));
                    string tempUserDir = FileIOHelper.BuildPath([FileIOHelper.TempPath(args.Settings.StorageRootPath), args.UserId], false);

                    // zip解凍
                    bool isUnziped = false;
                    if (args.SphrBinary != null && args.SphrBinary.Length > 0) {
                        isUnziped = ZipHelper.Unzip(args.SphrBinary, tempUserDir);
                    } else if (!string.IsNullOrWhiteSpace(args.SphrFilePath)) {
                        isUnziped = ZipHelper.Unzip(args.SphrFilePath, tempUserDir);
                    }

                    if (isUnziped) {
                        result.IsSuccess = this.ImportStorage(args.Settings, args.UserId, tempUserDir);
                        if (result.IsSuccess) {
                            LogHelper.Write("インポート完了しました。");
                        } else {
                            LogHelper.Write("インポート失敗しました。");
                        }
                    } else {
                        throw new InvalidOperationException(".sphrファイルの解凍に失敗しました。");
                    }
                } else {
                    throw new ArgumentNullException("引数がNull参照または不正です。: args");
                }
            } catch {
                throw;
            }

            return result;
        }

        #endregion

    }
}
