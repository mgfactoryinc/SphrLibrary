using System.Reflection;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// ログに関する機能を提供します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class LogHelper
    {
        #region "Variable"

        private static string rootPath = string.Empty;
        //private static string logFolder = Path.Combine(rootPath, SphrConst.LOG_FOLDER);

        private static readonly string logFileName = string.Format("{0}_{1}.log", DateTime.Now.ToString("dd"), Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().Name));

        private static string logFilePath
        {
            get { return Path.Combine(rootPath, SphrConst.LOG_FOLDER, LogHelper.logFileName); }
        }

        #endregion

        #region "Constructor"

        /// <summary>
        /// デフォルトコンストラクタは使用できません。
        /// </summary>
        private LogHelper() { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// ログ出力に関する機能を初期化します。
        /// </summary>
        /// <param name="rootPath">ルートパス。</param>
        /// <param name="func">デリゲートメソッド。</param>
        public static bool Initialize(string rootPath)
        {
            LogHelper.rootPath = rootPath;
            // なかったら作る
            return LogHelper.CheckLogFolder();
        }

        /// <summary>
        /// ログフォルダが存在するかどうか確認します。
        /// </summary>
        /// <returns></returns>
        public static bool CheckLogFolder()
        {
            bool result = true;
            string logDir = Path.Combine(rootPath, SphrConst.LOG_FOLDER);
            if (!Directory.Exists(logDir)) {
                try {
                    Directory.CreateDirectory(logDir);
                } catch {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// ログファイルを削除します。
        /// </summary>
        /// <returns></returns>
        public static bool Delete()
        {
            bool result = false;

            try {
                if (File.GetLastWriteTime(LogHelper.logFilePath).ToString("MM") != DateTime.Now.ToString("MM")) {
                    File.Delete(LogHelper.logFilePath);
                    result = true;
                }
            } catch { }

            return result;
        }

        /// <summary>
        /// ログファイルに書き込みます。
        /// </summary>
        /// <param name="message">出力文字列。</param>
        /// <returns></returns>
        public static bool Write(string message)
        {
            bool result = false;

            
            try {
                if (string.IsNullOrWhiteSpace(LogHelper.logFilePath)) throw new FileNotFoundException("ログファイルパスがNull参照または空白です。");
                //if (string.IsNullOrWhiteSpace(rootPath)) {
                //    rootPath = FileIOHelper.DefaultPath(Environment.OSVersion);
                //    LogHelper.Write("LogHelperが初期化されました。");
                //}
                using (StreamWriter writer = new StreamWriter(LogHelper.logFilePath, true, SphrConst.ENCODING)) {
                    writer.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), message.Trim()));
                    result = true;
                }
            } catch (Exception ex) {
                // ログ書き込みプロセスを掴めなかったら書き込み諦める
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        //public static bool Write(MethodBase? method, string message)
        //{
        //    bool result = false;

        //    if (method == null) {
        //        result = LogHelper.Write(message);
        //    } else {
        //        result = LogHelper.Write(string.Format("[{0}] {1}", method.Name, message));
        //    }

        //    return result;
        //}

        #endregion
    }
}
