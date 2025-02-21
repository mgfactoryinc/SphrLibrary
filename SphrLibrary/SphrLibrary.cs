using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Helpers;
using SphrLibrary.Workers;
using SphrLibrary.Workers.Args;
using SphrLibrary.Workers.Results;

namespace SphrLibrary
{
    /// <summary>
    /// SPHR 汎用モジュール(マネージド)に関する機能を提供します。
    /// </summary>
    public static class SphrLibrary
    {
        #region "Variable"

        /// <summary>
        /// 汎用モジュール 設定クラスを保持します。
        /// </summary>
        private static SphrLibrarySettings? Settings = null;

        /// <summary>
        /// 対象ユーザーIDを保持します。
        /// </summary>
        private static string? UserId = null;

        #endregion

        #region "Public Property"

        /// <summary>
        /// エクスポート時、データ抽出時のプロファイル情報を保持します。
        /// </summary>
        public static SphrProfile? Profile = null;

        /// <summary>
        /// 処理結果エラー情報を保持します。
        /// </summary>
        public static List<SphrResult>? Errors = null;

        /// <summary>
        /// 処理結果警告情報を保持します。
        /// </summary>
        public static List<SphrResult>? Warnings = null;

        #endregion

        #region "Private Method"

        
        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceName"></param>
        /// <param name="storageRootPath"></param>
        /// <param name="userId"></param>
        private static void Initialize(string serviceId, string serviceName, string storageRootPath, string userId)
        {
            SphrLibrary.Initialize(new SphrLibrarySettings(){
                ServiceId = serviceId,
                ServiceName = serviceName,
                StorageRootPath = storageRootPath,
                UserIdDigits = SphrConst.USER_ID_DIGITS_MIN
            });
            SphrLibrary.InitializeUser(userId);
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="settings"></param>
        private static void Initialize(SphrLibrarySettings settings) {

            if (settings != null && settings.IsValid()) {
                SphrLibrary.Settings = settings;

                // ログ機能初期化
                if (LogHelper.Initialize(Settings.StorageRootPath)) { 
                    LogHelper.Write(string.Format("[INITIALIZE] {0}", SphrHelper.GetVersion()));
                    LogHelper.Write(string.Format("[CONFIGURATION] {0}", new SphrJsonSerializer().Serialize<SphrLibrarySettings>(SphrLibrary.Settings)));
                } else {
                    Console.WriteLine("ログの初期化に失敗しました。");
                }
                
                // Tempフォルダのクリーン
                FileIOHelper.CleanTempFolder(Settings.StorageRootPath);
            } else if (SphrLibrary.Settings != null) {
                LogHelper.Write("settings: 設定値が不正です。");
            } else {
                Console.WriteLine("Settings: 設定値が不正です。");
            }
        }

        /// <summary>
        /// 汎用モジュール対象ユーザーIDを初期化します。
        /// </summary>
        /// <param name="userId"></param>
        private static void InitializeUser(string userId)
        {
            string previous = string.IsNullOrWhiteSpace(SphrLibrary.UserId) ? string.Empty : SphrLibrary.UserId;
            LogHelper.Write(string.Format("[USER ID] {0} -> {1}", previous, userId));
            SphrLibrary.UserId = userId;
        }

        #endregion

        #region "Public Method"

        
        /// <summary>
        /// 汎用モジュール バージョン情報を取得します。
        /// </summary>
        /// <returns></returns>
        public static string Version()
        {
            // 組み込み確認用。ログ初期化はパスが渡されてからなのでここでは吐かない
            return SphrHelper.GetVersion();
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceName"></param>
        /// <param name="storageRootPath"></param>
        /// <param name="userId"></param>
        public static void Init(string serviceId, string serviceName, string storageRootPath, string userId)
        {
            SphrLibrary.Initialize(serviceId, serviceName, storageRootPath, userId);
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceName"></param>
        /// <param name="storageRootPath"></param>
        public static void Init(string serviceId, string serviceName, string storageRootPath)
        {
            SphrLibrary.Initialize(serviceId, serviceName, storageRootPath, string.Empty);
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="userId"></param>
        public static void Init(string userId) {
            SphrLibrary.InitializeUser(userId);
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="settingsPath"></param>
        /// <param name="userId"></param>
        public static void Init(string settingsPath, Int64 userId) {
            SphrLibrarySettings settings = FileIOHelper.ReadSettings(settingsPath);

            if (settings.IsValid()) {
                SphrLibrary.Init(settings);
                int digits = settings.UserIdDigits < SphrConst.USER_ID_DIGITS_MIN ? SphrConst.USER_ID_DIGITS_MIN : settings.UserIdDigits;
                SphrLibrary.InitializeUser(userId.ToString().PadLeft(digits, '0'));

                LogHelper.Write(new SphrJsonSerializer().Serialize<SphrLibrarySettings>(settings));
            }
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool Init(SphrLibrarySettings settings)
        {
            SphrLibrary.Initialize(settings);
            return true;
        }

        /// <summary>
        /// 汎用モジュールを初期化します。
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool Init(SphrLibrarySettings settings, string userId)
        {
            SphrLibrary.Initialize(settings);
            SphrLibrary.InitializeUser(userId);
            return true;
        }

        /// <summary>
        /// エクスポート用 歩数データを格納します。
        /// </summary>
        /// <param name="physicalActivity"></param>
        /// <returns></returns>
        public static bool Set(PhysicalActivity physicalActivity)
        {
            try { 
                if (physicalActivity != null && physicalActivity.body != null && physicalActivity.body.Any()) {
                    if (SphrLibrary.Profile == null) SphrLibrary.Profile = new SphrProfile();
                    if (SphrLibrary.Profile.PhysicalActivities == null) SphrLibrary.Profile.PhysicalActivities = new Dictionary<ModalityTypeEnum, PhysicalActivity>();
                    // ここでのheaderはmodalityを受けるためだけにある
                    // エクスポート日時を合わせるためexportのときに再生成
                    ModalityTypeEnum modality = SphrHelper.GetModality(physicalActivity.header);
                    if (SphrLibrary.Profile.PhysicalActivities.ContainsKey(modality)) {
                        if (SphrLibrary.Profile.PhysicalActivities[modality] == null) SphrLibrary.Profile.PhysicalActivities[modality] = new PhysicalActivity();
                        if (SphrLibrary.Profile.PhysicalActivities[modality].body == null) SphrLibrary.Profile.PhysicalActivities[modality].body = [];
                        SphrLibrary.Profile.PhysicalActivities[modality].AddBody(physicalActivity.body);
                    } else {
                        SphrLibrary.Profile.PhysicalActivities.Add(modality, physicalActivity);
                    }
                    return true;
                }
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// エクスポート用 血圧データを格納します。
        /// </summary>
        /// <param name="bloodPressure"></param>
        /// <returns></returns>
        public static bool Set(BloodPressure bloodPressure)
        {
            try {
                if (bloodPressure != null && bloodPressure.body != null && bloodPressure.body.Any()) {
                    if (SphrLibrary.Profile == null) SphrLibrary.Profile = new SphrProfile();
                    if (SphrLibrary.Profile.BloodPressures == null) SphrLibrary.Profile.BloodPressures = new Dictionary<ModalityTypeEnum, BloodPressure>();
                    // ここでのheaderはmodalityを受けるためだけにある
                    // エクスポート日時を合わせるためexportのときに再生成
                    ModalityTypeEnum modality = SphrHelper.GetModality(bloodPressure.header);
                    if (SphrLibrary.Profile.BloodPressures.ContainsKey(modality)) {
                        if (SphrLibrary.Profile.BloodPressures[modality] == null) SphrLibrary.Profile.BloodPressures[modality] = new BloodPressure();
                        if (SphrLibrary.Profile.BloodPressures[modality].body == null) SphrLibrary.Profile.BloodPressures[modality].body = [];
                        SphrLibrary.Profile.BloodPressures[modality].AddBody(bloodPressure.body);
                    } else {
                        SphrLibrary.Profile.BloodPressures.Add(modality, bloodPressure);
                    }
                    return true;
                }
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// エクスポート用 体重データを格納します。
        /// </summary>
        /// <param name="bodyWeight"></param>
        /// <returns></returns>
        public static bool Set(BodyWeight bodyWeight)
        {
            try {
                // TODO
                //if (bodyWeights != null) {
                //    SphrLibrary.Profile!.BodyWeights = bodyWeights;
                //    return true;
                //}
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// エクスポート用 体温データを格納します。
        /// </summary>
        /// <param name="bodyTemperature"></param>
        /// <returns></returns>
        public static bool Set(BodyTemperature bodyTemperature)
        {
            try {
                // TODO
                //if (bodyTemperatures != null) {
                //    SphrLibrary.Profile!.BodyTemperatures = bodyTemperatures;
                //    return true;
                //}
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// エクスポート用 酸素飽和度データを格納します。
        /// </summary>
        /// <param name="oxygenSaturation"></param>
        /// <returns></returns>
        public static bool Set(OxygenSaturation oxygenSaturation)
        {
            try {
                // TODO
                //if (oxygenSaturations != null) {
                //    SphrLibrary.Profile!.OxygenSaturations = oxygenSaturations;
                //    return true;
                //}
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return false; 
        }

        /// <summary>
        /// エクスポートを実行します。
        /// </summary>
        /// <param name="exportDate"></param>
        /// <returns></returns>
        public static string Export(DateTime exportDate)
        {
            string result = string.Empty;
            SphrExportWorkerResults results = new SphrExportWorker().TryExecute(
                new SphrExportWorkerArgs(SphrLibrary.Settings, SphrLibrary.UserId, SphrLibrary.Profile) { ExportDate = exportDate });

            if (results != null) {
                if (results.IsSuccess) {
                    result = results.ExportFilePath;
                } else if (results.Results != null && results.Results.Any()) {
                    SphrLibrary.Errors = new List<SphrResult>();
                    SphrLibrary.Errors.AddRange(results.Results);
                } else { 
                    
                }
            } else { 
            
            }
            return result;

        }

        /// <summary>
        /// ファイルパスを指定して、インポートを実行します。
        /// </summary>
        /// <param name="sphrFilePath"></param>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static bool Import(string sphrFilePath)
        {
            bool result = false;
            SphrImportWorkerResults results = new SphrImportWorker().TryExecute(
                new SphrImportWorkerArgs(SphrLibrary.Settings, SphrLibrary.UserId, SphrLibrary.Profile) { SphrFilePath = sphrFilePath });

            if (results != null) {
                result = results.IsSuccess;
                if (results.IsSuccess) {
                    LogHelper.Write("import success");
                } else if (results.Results != null && results.Results.Any()) {
                    SphrLibrary.Errors = new List<SphrResult>();
                    SphrLibrary.Errors.AddRange(results.Results);

                    SphrLibrary.Errors.ForEach(i => LogHelper.Write(string.Format("{0}:{1}", i.code, i.detail)));
                } else {
                    LogHelper.Write("import failure.");
                }
            } else { LogHelper.Write("results is null."); }

            return result;
        }

        /// <summary>
        /// バイナリデータを指定して、インポートを実行します。
        /// </summary>
        /// <param name="sphrBinary"></param>
        /// <param name="fileName"></param>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static bool Import(byte[] sphrBinary, string fileName)
        {
            bool result = false;
            SphrImportWorkerResults results = new SphrImportWorker().TryExecute(
                new SphrImportWorkerArgs(SphrLibrary.Settings, SphrLibrary.UserId, SphrLibrary.Profile) { SphrBinary = sphrBinary, FileName = fileName });

            if (results != null) {
                result = results.IsSuccess;
                if (results.IsSuccess) {
                    LogHelper.Write("import success");
                } else if (results.Results != null && results.Results.Any()) {
                    SphrLibrary.Errors = new List<SphrResult>();
                    SphrLibrary.Errors.AddRange(results.Results);

                    SphrLibrary.Errors.ForEach(i => LogHelper.Write(string.Format("{0}:{1}", i.code, i.detail)));
                } else {
                    LogHelper.Write("import failure.");
                }
            } else { LogHelper.Write("results is null."); }

            return result;

        }

        /// <summary>
        /// データ抽出を実行します。
        /// </summary>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static SphrProfile? Extract(DocumentReferenceTypeEnum extractType) { 
            
            SphrProfile? result = null;
            SphrExtractWorkerResults results = new SphrExtractWorker().TryExecute(
                new SphrExtractWorkerArgs(SphrLibrary.Settings, SphrLibrary.UserId, SphrLibrary.Profile) { ExtractType = extractType });

            if (results != null) {
                if (results.IsSuccess) {
                    result = results.Profile;
                    SphrLibrary.Profile = results.Profile;
                } else if (results.Results != null && results.Results.Any()) {
                    SphrLibrary.Errors = new List<SphrResult>();
                    SphrLibrary.Errors.AddRange(results.Results);

                    SphrLibrary.Errors.ForEach(i => Console.WriteLine("{0}:{1}", i.code, i.detail));
                } else {
                }
            }

            return result;
        }

        /// <summary>
        /// 非同期でエクスポートを実行します。
        /// </summary>
        /// <param name="exportDate"></param>
        /// <returns></returns>
        public static async Task<string> ExportAsync(DateTime exportDate)
        {
            return await Task.Run(() => SphrLibrary.Export(exportDate));
        }

        /// <summary>
        /// 非同期でエクスポートを実行します。
        /// </summary>
        /// <param name="exportDate"></param>
        /// <param name="callback"></param>
        public static async void ExportAsync(DateTime exportDate, Action<Task<bool>> callback)
        {
            await Task.Run(() => SphrLibrary.Export(exportDate)).ContinueWith((t) => callback).ConfigureAwait(false);
        }

        /// <summary>
        /// 非同期でインポートを実行します。
        /// </summary>
        /// <param name="sphrFilePath"></param>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static async Task<bool> ImportAsync(string sphrFilePath)
        {
            return await Task.Run(() => SphrLibrary.Import(sphrFilePath));
        }

        /// <summary>
        /// 非同期でインポートを実行します。
        /// </summary>
        /// <param name="sphrFilePath"></param>
        /// <param name="extractType"></param>
        /// <param name="callback"></param>
        public static async void ImportAsync(string sphrFilePath, Action<Task<bool>> callback)
        {
            await Task.Run(() => SphrLibrary.Import(sphrFilePath)).ContinueWith((t) => callback).ConfigureAwait(false);
        }

        /// <summary>
        /// 非同期でインポートを実行します。
        /// </summary>
        /// <param name="sphrBinary"></param>
        /// <param name="fileName"></param>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static async Task<bool> ImportAsync(byte[] sphrBinary, string fileName)
        {
            return await Task.Run(() => SphrLibrary.Import(sphrBinary, fileName));
        }

        /// <summary>
        /// 非同期でインポートを実行します。
        /// </summary>
        /// <param name="sphrBinary"></param>
        /// <param name="fileName"></param>
        /// <param name="extractType"></param>
        /// <param name="callback"></param>
        public static async void ImportAsync(byte[] sphrBinary, string fileName, Action<Task<bool>> callback)
        {
            await Task.Run(() => SphrLibrary.Import(sphrBinary, fileName)).ContinueWith((t) => callback).ConfigureAwait(false);
        }

        /// <summary>
        /// 非同期でデータ抽出を実行します。
        /// </summary>
        /// <param name="extractType"></param>
        /// <returns></returns>
        public static async Task<SphrProfile?> ExtractAsync(DocumentReferenceTypeEnum extractType)
        {
            return await Task.Run(() => SphrLibrary.Extract(extractType));
        }

        /// <summary>
        /// 非同期でデータ抽出を実行します。
        /// </summary>
        /// <param name="extractType"></param>
        /// <param name="callback"></param>
        public static async void ExtractAsync(DocumentReferenceTypeEnum extractType, Action<Task<SphrProfile?>> callback)
        {
            await Task.Run(() => SphrLibrary.Extract(extractType)).ContinueWith((t) => callback).ConfigureAwait(false);
        }

        #endregion
    }
}
