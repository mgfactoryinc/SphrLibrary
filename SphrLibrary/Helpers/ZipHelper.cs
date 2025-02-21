using System.IO.Compression;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// Zip圧縮解凍に関する機能を提供します。
    /// </summary>
    internal static class ZipHelper
    {
        #region "Public Method"
        
        /// <summary>
        /// Zip圧縮を行います。
        /// </summary>
        /// <param name="sourceFolderPath">圧縮元フォルダパス。</param>
        /// <param name="destinationFilePath">保存先ファイルパス。</param>
        /// <param name="includeBaseDirectory">ベースディレクトリを含めるかどうか(def:含めない)。</param>
        /// <returns></returns>
        public static bool Zip(string sourceFolderPath, string destinationFilePath, bool includeBaseDirectory = false)
        {
            bool result = false;

            try {
                ZipFile.CreateFromDirectory(sourceFolderPath, destinationFilePath, CompressionLevel.Optimal, includeBaseDirectory, SphrConst.ENCODING);
                result = true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return result;
        }

        /// <summary>
        /// ファイルパスを指定して、Zip解凍を行います。
        /// </summary>
        /// <param name="sphrFilePath">解凍ファイルパス。</param>
        /// <param name="destinationFolderPath">展開先フォルダパス。</param>
        /// <param name="overwriteFiles">展開先にファイルが存在する場合、上書きするかどうか(def:上書きしない)。</param>
        /// <returns></returns>
        public static bool Unzip(string sphrFilePath, string destinationFolderPath, bool overwriteFiles = false)
        {
            bool result = false;

            try {
                ZipFile.ExtractToDirectory(sphrFilePath, destinationFolderPath, SphrConst.ENCODING, overwriteFiles);
                result = true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// バイナリを指定して、Zip解凍を行います。
        /// </summary>
        /// <param name="sphrBinary">バイナリデータ。</param>
        /// <param name="destinationFolderPath">展開先フォルダパス。</param>
        /// <param name="overwriteFiles">展開先にファイルが存在する場合、上書きするかどうか(def:上書きしない)。</param>
        /// <returns></returns>
        public static bool Unzip(byte[] sphrBinary, string destinationFolderPath, bool overwriteFiles = false)
        {
            bool result = false;

            try {
                using (Stream s = new MemoryStream(sphrBinary)) {
                    ZipFile.ExtractToDirectory(s, destinationFolderPath, SphrConst.ENCODING, overwriteFiles);
                }
                result = true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return result;
        }

        #endregion
    }
}
