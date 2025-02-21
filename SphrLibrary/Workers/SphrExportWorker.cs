using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Extensions;
using SphrLibrary.Helpers;
using SphrLibrary.Workers.Args;
using SphrLibrary.Workers.Base;
using SphrLibrary.Workers.Results;

namespace SphrLibrary.Workers
{
    /// <summary>
    /// 汎用モジュールが使用するエクスポートに関する機能を提供します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExportWorker : SphrWorkerBase<SphrExportWorkerArgs, SphrExportWorkerResults>
    {
        #region "Constructor"

        /// <summary>
        /// <see cref="SphrExportWorker"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrExportWorker() : base(SphrOperationTypeEnum.Export) { }

        #endregion

        #region "Private Method"

        //internal static bool Export(DateTime exportDate)
        //{
        //    bool result = false;
        //    //string path = SphrLibrary.Export();

        //    //Console.WriteLine(SphrLibrary.SphrLibrary.Profile.StorageRootPath);
        //    //Console.WriteLine("bp.count:{0}", SphrLibrary.SphrLibrary.Profile.BloodPressures!.Count);
        //    //Console.WriteLine("pa.count:{0}", SphrLibrary.SphrLibrary.Profile.PhysicalActivities!.Count);
        //    //Int64 result = 0;
        //    SphrExportWorkerResults results = new SphrExportWorker().TryExecute(new() { Profile = SphrLibrary.Profile, ExportDate = exportDate });
        //    //Console.WriteLine(results.ToString());

        //    if (results != null) {
        //        //Console.WriteLine(results.IsSuccess);
        //        if (results.IsSuccess) {
        //            LogHelper.Write(exportDate.ToDateString());
        //            // ミリ秒以下7桁->3桁にする
        //            Int64 ticks = exportDate.ToUniversalTime().Ticks / 10000;
        //            Int64 preset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
        //            LogHelper.Write(string.Format("{0} - {1} = {2}", ticks, preset, ticks - preset));

        //            //result = ticks - preset;
        //            //result = exportDate.ToString("yyyyMMddHHmmssfff").ToValueType<Int64>();
        //            //result = results.ExportFilePath;

        //            result = true;
        //        }
        //        else if (results.Results != null && results.Results.Any()) {
        //            SphrLibrary.Errors = new List<SphrResult>();
        //            SphrLibrary.Errors.AddRange(results.Results);
        //            results.Results.ForEach((x) => LogHelper.Write(string.Format("{0}:{1}", x.code, x.detail)));
        //        } else {

        //        }
        //    } else {

        //    }
        //    return result;
        //}

        #endregion

        #region "Public Method"

        /// <summary>
        /// エクスポートを実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public override SphrExportWorkerResults Execute(SphrExportWorkerArgs args)
        {
            SphrExportWorkerResults result = new SphrExportWorkerResults();

            try {
                if (args != null) {
                    if (args.Settings != null && args.Settings.IsValid() && !string.IsNullOrWhiteSpace(args.UserId)) {
                        LogHelper.Write(string.Format("エクスポートを開始します。: {0}", args.UserId));
                        DateTime exportDate = (args.ExportDate != DateTime.MinValue) ? args.ExportDate : DateTime.Now;
                        string work = FileIOHelper.WorkingFolder(args.Settings.ServiceId, exportDate);
                        string userDir = FileIOHelper.SourcePath(args.Settings.StorageRootPath, args.UserId);
                        string exportDir = Path.Combine(userDir, work);

                        if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);
                        string temp = FileIOHelper.TempPath(args.Settings.StorageRootPath);
                        string fileName = string.Format("SPHR_{0}{1}", work, SphrConst.SPHR_FILE_EXTENSION);

                        // 自サービスデータ書き出し
                        string value = SphrHelper.Write(args.Settings, args.Profile, exportDir, DateTime.Now);

                        // zip圧縮（過去にインポートされたものも同階層にあるのでまとめて）
                        string zipPath = Path.Combine(temp, work);
                        string saveFileName = Path.Combine(temp, fileName);
                        FileIOHelper.CopyDirectory(userDir, zipPath, true);

                        if (ZipHelper.Zip(zipPath, saveFileName)) {
                            FileIOHelper.DeleteDirectory(zipPath);
                            result.ExportFilePath = saveFileName;
                            result.IsSuccess = true;
                            LogHelper.Write("エクスポート完了しました。");
                        } else {
                            LogHelper.Write(".sphrファイル圧縮に失敗しました。");
                        }
                    } else {
                        throw new ArgumentException("エクスポート設定が不足しています。");
                    }
                } else {
                    throw new ArgumentNullException(nameof(args), "引数がNull参照です。");
                }
            } catch {
                throw;
            }

            return result;
        }

        #endregion

    }
}
