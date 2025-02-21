using Json.Schema;
using SphrLibrary.Entities.FHIR;
using SphrLibrary.Entities.SPHR;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// ファイルI/Oに関する機能を提供します。
    /// </summary>
    internal static class FileIOHelper
    {
        #region "Public Method"

        /// <summary>
        /// SPHRインデックス情報を開きます。
        /// </summary>
        /// <param name="path">ファイルパス。</param>
        /// <returns>SPHRインデックスクラス。</returns>
        public static IndexPhr OpenIndexPhr(string path)
        {
            LogHelper.Write(string.Format("FileIOHelper.OpenIndexPhr: {0}", path));

            IndexPhr result = new IndexPhr();

            try {
                if (File.Exists(path)) {
                    using (StreamReader reader = new StreamReader(path)) {
                        SphrJsonSerializer serializer = new SphrJsonSerializer();
                        int index = 1;
                        do {
                            // NDJson(改行区切りのJson) -> 行単位ではJsonだけど、全体はJson形式ではない
                            string? line = reader.ReadLine();
                            if (line != null) {
                                switch (index) {
                                    case 1:
                                        // Composition
                                        Composition? composition = serializer.Deserialize<Composition>(line);
                                        if (composition != null) result.Composition = composition;
                                        break;
                                    case 2:
                                        // DocumentManifest
                                        DocumentManifest? manifest = serializer.Deserialize<DocumentManifest>(line);
                                        if (manifest != null) result.DocumentManifest = manifest;
                                        break;
                                    default:
                                        // DocumentReference
                                        DocumentReference? reference = serializer.Deserialize<DocumentReference>(line);
                                        if (reference != null) result.DocumentReferences.Add(reference);
                                        break;
                                }
                            } else {
                                LogHelper.Write("Stream is null.");
                            }
                            index++;
                        } while (reader.Peek() > 0);
                    }
                } else {
                    LogHelper.Write("index.phr is not found.");
                }
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// SPHRインデックス情報を保存します。
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="savePath"></param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        public static bool SaveIndexPhr(IndexPhr entity, string savePath)
        {
            bool result = false;

            try {
                SphrJsonSerializer serializer = new SphrJsonSerializer();
                string filePath = Path.Combine(savePath, "index.phr");
                using (StreamWriter writer = new StreamWriter(filePath, false, SphrConst.ENCODING)) {
                    // NDJson(改行区切りのJson) -> 行単位ではJsonだけど、全体はJson形式ではない
                    writer.WriteLine(serializer.Serialize<Composition>(entity.Composition));
                    writer.Write(serializer.Serialize<DocumentManifest>(entity.DocumentManifest));
                    if (entity.DocumentReferences != null) {
                        foreach (DocumentReference reference in entity.DocumentReferences) {
                            writer.WriteLine();
                            writer.Write(serializer.Serialize<DocumentReference>(reference));
                        }
                    }
                }
                result = true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 対象ユーザーIDのストレージパスを取得します。
        /// </summary>
        /// <param name="rootPath">ストレージルートパス。</param>
        /// <param name="userId">ユーザーID。</param>
        /// <param name="workingFolder">作業フォルダパス。</param>
        /// <returns>ユーザーIDのストレージパス。</returns>
        public static string SourcePath(string rootPath, string userId, string workingFolder = "")
        {
            string tempId = userId;
            if (tempId.Length < 8) tempId = tempId.PadLeft(8, '0');

            List<string> paths = new List<string>() { rootPath, SphrConst.SPHRDB_FOLDER };
            for (int i = 0; i < SphrConst.USER_ID_STORAGE_LAYERS; i++) {
                paths.Add(tempId.Substring(i * SphrConst.USER_ID_STORAGE_LAYER_LENGTH, SphrConst.USER_ID_STORAGE_LAYER_LENGTH));
            }
            paths.Add(userId);
            if (workingFolder != null) paths.Add(workingFolder);

            string result = Path.Combine(paths.ToArray());

            //debug
            //Console.WriteLine("SourcePath:{0}", result);

            // なかったら作る
            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }

        /// <summary>
        /// Tempフォルダパスを取得します。
        /// </summary>
        /// <param name="rootPath">ストレージルートパス。</param>
        /// <returns>Tempフォルダパス。</returns>
        public static string TempPath(string rootPath)
        {
            string result = Path.Combine(rootPath, SphrConst.TEMP_FOLDER);

            //debug
            //Console.WriteLine("TempPath:{0}", result);

            // なかったら作る
            if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }

        //public static string TempPath(string rootPath, string userId)
        //{
        //    string result = Path.Combine(rootPath, SphrConst.TEMP_FOLDER, userId);
        //    if (!Directory.Exists(result)) Directory.CreateDirectory(result);
        //    return result;
        //}

        /// <summary>
        /// 作業フォルダパスを取得します。
        /// </summary>
        /// <param name="serviceId">サービスID。</param>
        /// <param name="exportDate">エクスポート日時。</param>
        /// <returns>作業フォルダパス。</returns>
        public static string WorkingFolder(string serviceId, DateTime exportDate)
        {
            //! [サービスID]_[エクスポート日時(2024093010225530)]
            string result = string.Format("{0}_{1}", serviceId, exportDate.ToString("yyyyMMddHHmmssfff"));

            //debug
            //Console.WriteLine("WorkingFolder:{0}", result);

            // なかったら作る
            //if (!Directory.Exists(result)) Directory.CreateDirectory(result);

            return result;
        }

        /// <summary>
        /// フォルダを削除します。
        /// </summary>
        /// <param name="path">フォルダパス。</param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        public static bool DeleteDirectory(string path)
        {
            try {
                if (Directory.Exists(path)) Directory.Delete(path, true); return true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// <param name="path">ファイルパス。</param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        public static bool DeleteFile(string path)
        {
            try {
                if (File.Exists(path)) File.Delete(path); return true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Tempフォルダをクリーンアップします。
        /// </summary>
        /// <param name="rootPath">ストレージルートパス。</param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        public static bool CleanTempFolder(string rootPath)
        {
            const int EXPIRES_DAYS = 30;
            bool result = false;

            try {
                string tempPath = FileIOHelper.TempPath(rootPath);

                if (Directory.Exists(tempPath)) {
                    foreach (string dirPath in Directory.GetDirectories(tempPath)) { 
                        if (Directory.GetLastWriteTime(dirPath)!.AddDays(EXPIRES_DAYS) < DateTime.Now) { 
                            FileIOHelper.DeleteDirectory(dirPath);
                        }
                    }
                    foreach (string filePath in Directory.GetFiles(tempPath, "*", SearchOption.TopDirectoryOnly)) {
                        if (File.GetLastWriteTime(filePath)!.AddDays(EXPIRES_DAYS) < DateTime.Now) { 
                            FileIOHelper.DeleteFile(filePath);
                        }
                    }
                }
                result = true;

            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return result;
        }

        /// <summary>
        /// パスを構築します。
        /// </summary>
        /// <param name="paths">パスの要素の配列。</param>
        /// <param name="isDeleteExists">既に存在する場合、削除するかどうか(def:削除する)。</param>
        /// <param name="isCreate">フォルダが存在しない場合、作成するかどうか(def:作成する)。</param>
        /// <returns></returns>
        public static string BuildPath(string[] paths, bool isDeleteExists = true, bool isCreate = true)
        {
            string result = Path.Combine(paths);

            if (isDeleteExists && Directory.Exists(result)) FileIOHelper.DeleteDirectory(result);
            if (isCreate && !Directory.Exists(result)) Directory.CreateDirectory(result);
            return result;
        }

        /// <summary>
        /// ディレクトリをコピーします。
        /// </summary>
        /// <param name="sourceDir">コピー元ディレクトリ。</param>
        /// <param name="destinationDir">コピー先ディレクトリ。</param>
        /// <param name="recursive">サブフォルダを再帰的にコピーするかどうか。</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles()) {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive) {
                foreach (DirectoryInfo subDir in dirs) {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        /// <summary>
        /// 汎用モジュール設定ファイルを読み込みます。
        /// </summary>
        /// <param name="path">設定Jsonファイルパス。</param>
        /// <returns>汎用モジュール設定クラス。</returns>
        public static SphrLibrarySettings ReadSettings(string path) {

            SphrLibrarySettings result = new SphrLibrarySettings();

            try {
                if (File.Exists(path)) {
                    using (StreamReader reader = new StreamReader(path)) {
                        result = new SphrJsonSerializer().Deserialize<SphrLibrarySettings>(reader.ReadToEnd());
                    }
                }
            } catch (Exception ex) {
                // ログのパスが確定しないのでコンソールに表示
                Console.WriteLine(ex.Message);
            }
            
            return result;
        }

        /// <summary>
        /// ストレージのデフォルトパスを取得します。
        /// </summary>
        /// <param name="os">OS情報。</param>
        /// <returns></returns>
        public static string DefaultPath(OperatingSystem os) {

            string result = string.Empty;
            switch (os.Platform) {
                case PlatformID.Unix:
                    result = "/sphr";
                    break;
                case PlatformID.Win32NT:
                    result = "C:\\sphr";
                    break;
                default:
                    break;
            }

            return result;
        }

        #endregion
    }
}
