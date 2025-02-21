using SphrLibrary.Entities.OpenmHealth;
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
    /// 汎用モジュールが使用するデータ抽出に関する機能を提供します。
    /// このクラスは継承できません。
    /// </summary>
    internal sealed class SphrExtractWorker : SphrWorkerBase<SphrExtractWorkerArgs, SphrExtractWorkerResults>
    {
        #region "Constructor"

        /// <summary>
        /// <see cref="SphrExtractWorker"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrExtractWorker() : base(SphrOperationTypeEnum.Extract) { }

        #endregion

        #region "Private Method"
        
        /// <summary>
        /// データを抽出します。
        /// </summary>
        /// <param name="profile">SPHRプロファイルクラス。</param>
        /// <param name="userDir">対象ユーザーのインポート済みフォルダパス。</param>
        /// <param name="extractType">データ抽出項目種別。</param>
        /// <returns>成功ならtrue、失敗ならfalse。</returns>
        private bool Extract(out SphrProfile profile, string userDir, DocumentReferenceTypeEnum extractType)
        {
            LogHelper.Write("SphrExtractWorker.Extract");

            // インポート済みのデータ取得
            List<SphrProfile> list = SphrHelper.Read(userDir, extractType);

            LogHelper.Write(string.Format("list.count: {0}", list.Count));

            // 重複チェック（サービスごとのデータで重複があればマージ）
            profile = this.Merge(list)!;

            return profile != null;

        }

        /// <summary>
        /// サービサー間の重複データをマージします。
        /// </summary>
        /// <param name="profiles"></param>
        /// <returns>SPHRプロファイルクラス。</returns>
        private SphrProfile? Merge(List<SphrProfile> profiles)
        {
            SphrProfile? result = null;
            
            if (profiles != null && profiles.Any()) {
                foreach (SphrProfile profile in profiles) {
                    if (profile != null) {
                        if (result == null) {
                            // １つ目は無条件
                            result = profile!;
                        } else {
                            // 血圧
                            if (result.BloodPressures == null) result.BloodPressures = new Dictionary<ModalityTypeEnum, BloodPressure>();
                            if (profile.BloodPressures != null) {
                                foreach (KeyValuePair<ModalityTypeEnum, BloodPressure> kvp in profile.BloodPressures) {
                                    if (result.BloodPressures.ContainsKey(kvp.Key)) {
                                        foreach (BloodPressureBody body in kvp.Value.body) {
                                            int index = result.BloodPressures[kvp.Key].body.ToList().FindIndex(x => 
                                                x.effective_time_frame != null && body.effective_time_frame != null &&
                                                x.effective_time_frame.date_time == body.effective_time_frame.date_time);
                                            if (index >= 0) { 
                                                // headerのsource_creation_date_timeの新しいほうを採用
                                                if (result.BloodPressures[kvp.Key].header.source_creation_date_time.TryToValueType(DateTime.MinValue) < kvp.Value.header.source_creation_date_time.TryToValueType(DateTime.MinValue)) {
                                                    result.BloodPressures[kvp.Key].body[index] = body;
                                                }
                                            } else {
                                                result.BloodPressures[kvp.Key].AddBody([body]);
                                            }
                                        }
                                    } else {
                                        result.BloodPressures.Add(kvp.Key, kvp.Value);
                                    }
                                }
                            }
                            
                            // 歩数
                            if (result.PhysicalActivities == null) result.PhysicalActivities = new Dictionary<ModalityTypeEnum, PhysicalActivity>();
                            if (profile.PhysicalActivities != null) {
                                foreach (KeyValuePair<ModalityTypeEnum, PhysicalActivity> kvp in profile.PhysicalActivities) {
                                    if (result.PhysicalActivities.ContainsKey(kvp.Key)) {
                                        foreach (PhysicalActivityBody body in kvp.Value.body) {
                                            int index = result.BloodPressures[kvp.Key].body.ToList().FindIndex(x => 
                                                x.effective_time_frame != null && x.effective_time_frame.time_interval != null && body.effective_time_frame != null && body.effective_time_frame.time_interval != null &&
                                                x.effective_time_frame.time_interval.start_date_time == body.effective_time_frame.time_interval.start_date_time &&
                                                x.effective_time_frame.time_interval.end_date_time == body.effective_time_frame.time_interval.end_date_time);
                                            if (index >= 0) { 
                                                // headerのsource_creation_date_timeの新しいほうを採用
                                                if (result.PhysicalActivities[kvp.Key].header.source_creation_date_time.TryToValueType(DateTime.MinValue) < kvp.Value.header.source_creation_date_time.TryToValueType(DateTime.MinValue)) {
                                                    result.PhysicalActivities[kvp.Key].body[index] = body;
                                                } else { 
                                                    // 元が新しいので何もしない
                                                }
                                            } else {
                                                result.PhysicalActivities[kvp.Key].AddBody([body]);
                                            }
                                        }
                                    } else {
                                        result.PhysicalActivities.Add(kvp.Key, kvp.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            if (result == null) {
                LogHelper.Write("SphrExtractWorker.Merge: result is null");
            } else {
                if (result.PhysicalActivities != null) LogHelper.Write(string.Format("SphrExtractWorker.Merge: PhysicalActivity.Count: {0}", result.PhysicalActivities.Count));
                if (result.BloodPressures != null) LogHelper.Write(string.Format("SphrExtractWorker.Merge: BloodPressure.Count: {0}", result.BloodPressures.Count));
            }

            return result;
        }

        #endregion

        #region "Public Method"

        /// <summary>
        /// データ抽出を実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public override SphrExtractWorkerResults Execute(SphrExtractWorkerArgs args)
        {
            SphrExtractWorkerResults result = new SphrExtractWorkerResults() { IsSuccess = false };

            try {
                if (args != null) {
                    if (args.Settings != null && args.Settings.IsValid() && !string.IsNullOrWhiteSpace(args.UserId)) {
                        LogHelper.Write(string.Format("データ抽出を開始します。: {0}", args.UserId));

                        string userDir = FileIOHelper.SourcePath(args.Settings.StorageRootPath, args.UserId);

                        // データ抽出実行
                        result.IsSuccess = this.Extract(out SphrProfile profile, userDir, args.ExtractType);

                        if (result.IsSuccess) {
                            result.Profile = profile;
                            LogHelper.Write("データ抽出完了しました。");
                        } else {
                            LogHelper.Write("データ抽出失敗しました。");
                        }
                    } else {
                        throw new ArgumentException("データ抽出設定が不足しています。");
                    }
                } else {
                    throw new ArgumentNullException("引数がNull参照です。: args");
                }
            } catch {
                throw;
            }

            return result;
        }

        #endregion

    }
}
