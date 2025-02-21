using SphrLibrary.Entities.FHIR;
using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// 汎用モジュールのヘルパー機能を提供します。
    /// </summary>
    internal static class SphrHelper
    {
        /// <summary>
        /// 汎用モジュール バージョン情報を取得します。
        /// </summary>
        /// <returns></returns>
        internal static string GetVersion()
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name!;
            Version ver = Assembly.GetExecutingAssembly().GetName().Version!;

            return string.Format("{0}<{1}.{2}.{3}>", name, ver.Major, ver.Minor, ver.Build);
        }

        /// <summary>
        /// マネージド オブジェクトからアンマネージド メモリ ブロックにデータをマーシャリングします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        [RequiresDynamicCode("Calls System.Runtime.InteropServices.Marshal.SizeOf(Type)")]
        internal static IntPtr ToPtr<T>(T obj) where T : class
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        /// <summary>
        /// エラーメッセージを構築します。
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        internal static void BuildExceptionMessage(ref StringBuilder msg, Exception ex)
        {
            if (msg == null) msg = new StringBuilder();
            if (ex != null) {
                msg.AppendFormat("{0}; ", ex.Message);
                if (ex.InnerException != null) BuildExceptionMessage(ref msg, ex.InnerException);
            }
        }
        
        /// <summary>
        /// エラーコードを構築します。
        /// </summary>
        /// <param name="workerName"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        internal static string BuildErrorCode(string workerName, int errorType)
        {
            return string.Join(".", ["e", "sphr", workerName.ToLower(), errorType.ToString("d3")]);
        }

        /// <summary>
        /// 処理結果コードを構築します。
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="operationTypeEnum"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static string BuildResultCode(SphrResultCodeTypeEnum codeType, SphrOperationTypeEnum operationTypeEnum, int code)
        {
            return string.Join(".", [codeType.ToString()[..1].ToLower(), "sphr", operationTypeEnum.ToString().ToLower(), code.ToString("d3")]);
        }

        /// <summary>
        /// 処理結果クラスを構築します。
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="operationTypeEnum"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static SphrResult BuildResult(SphrResultCodeTypeEnum codeType, SphrOperationTypeEnum operationTypeEnum, int code, string message)
        {
            return new SphrResult() { 
                code = SphrHelper.BuildResultCode(codeType, operationTypeEnum, code),
                detail = message
            };
        }

        /// <summary>
        /// 処理結果クラス（エラー）を構築します。
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="ex"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static SphrResult? BuildErrorResult<TException>(TException ex, string name, int code = 999) where TException : Exception
        {
            SphrResult? result = null;

            try {
                StringBuilder msg = new StringBuilder();
                SphrHelper.BuildExceptionMessage(ref msg, ex);

                // Log
                LogHelper.Write(msg.ToString());
                if (!string.IsNullOrWhiteSpace(ex.Source)) LogHelper.Write(ex.Source);
                if (!string.IsNullOrWhiteSpace(ex.StackTrace)) LogHelper.Write(ex.StackTrace);
                //if (ex.TargetSite != null) LogHelper.Write(ex.TargetSite.ToString());

                result = new SphrResult() {
                    code = SphrHelper.BuildErrorCode(name, code),
                    detail = msg.ToString(),
                };
            } catch {}

            return result;
        } 

        /// <summary>
        /// index.phr Composition プロファイルを構築します。
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceName"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        private static Composition BuildComposition(string serviceId, string serviceName, DateTime creationDate)
        {
            return new Composition() {
                resourceType = ResourceTypeEnum.Composition.ToString(),
                author = [
                    new Author() { reference = string.Format("Organization/{0}", serviceId) },
                    //new Author() { inner = new AuthorInner() {
                    //    reference = string.Format("Organization/{0}", serviceId)
                    //}}
                ],
                contained = [new Contained() { 
                    resourceType = "Organization",
                    id = serviceId,
                    name = serviceName
                }],
                date = creationDate.ToDateString(),
                status = CompositionStatusTypeEnum.final.ToString(),
                title = string.Format("ExportPHR{0}", creationDate.ToString("yyyyMMdd")),
                type = new CodableConcept() { 
                    coding = [
                        new Coding() {
                            code = "34133-9",
                            display = "Summary of episode note",
                            system = "http://loinc.org"
                        }
                    ]
                }
            };
        }

        /// <summary>
        /// index.phr DocumentManifest プロファイルを構築します。
        /// </summary>
        /// <param name="manifestUuid"></param>
        /// <param name="referenceUuids"></param>
        /// <returns></returns>
        private static DocumentManifest BuildDocumentManifest(Guid manifestUuid, List<Guid> referenceUuids)
        {
            return new DocumentManifest() { 
                resourceType = ResourceTypeEnum.DocumentManifest.ToString(),
                id = manifestUuid.ToGuidString(),
                content = [.. referenceUuids.ConvertAll(x => new Reference() { reference = x.ToGuidString() })],
                // content = referenceUuids.ConvertAll<Reference<int>>((i) => {
                //     return new Reference<int>() { reference = i.ToGuidString() };
                // }).ToArray<Reference<int>>(),
                status = DocumentStatusTypeEnum.current.ToString(),
            };
        }

        /// <summary>
        /// index.phr DocumentReference プロファイルを構築します。
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="dataType"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static DocumentReference BuildDocumentReference(Guid uuid, DocumentReferenceTypeEnum dataType, string fileName, string contentType)
        {
            return new DocumentReference() { 
                resourceType = ResourceTypeEnum.DocumentReference.ToString(),
                id = uuid.ToGuidString(),
                type = new CodableConcept() {
                    coding = [ SphrConst.CODE_SYSTEM[dataType] ]
                },
                content = [
                    new BackboneElement() {
                        attachment = new Attachment() {
                            contentType = contentType,
                            //language = "ja-jp",
                            //creation = fileCreationDate.ToDateString(),
                            //hash = fileHash,
                            //size = fileSize,
                            //title = "",
                            url = fileName
                        } 
                    } 
                ],
                status = DocumentStatusTypeEnum.current.ToString(),
            };
        }

        /// <summary>
        /// コードシステムからリソースタイプを取得します。
        /// </summary>
        /// <param name="sourceCodeSystem"></param>
        /// <returns></returns>
        private static DocumentReferenceTypeEnum GetResourceType(string sourceCodeSystem)
        {
            DocumentReferenceTypeEnum result = DocumentReferenceTypeEnum.None;
            foreach (KeyValuePair<DocumentReferenceTypeEnum, Coding> kvp in SphrConst.CODE_SYSTEM) { 
                if (kvp.Value.CodeSystem().CompareTo(sourceCodeSystem) == 0) { 
                    result = kvp.Key;
                    break;
                }
            }
            return result;
        }

        //internal static string BuildUserIdStoragePath(string rootPath, string userId)
        //{
        //    string path = Path.Combine(rootPath, userId);
        //    return path;
        //} 

        /// <summary>
        /// モダリティ（計測機器）を取得します。
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        internal static ModalityTypeEnum GetModality(OmhHeader header)
        {
            // TODO モダリティが不明な場合はnull。nullの場合は自己報告と扱われる
            ModalityTypeEnum result = ModalityTypeEnum.self_reported;
            if (header != null && header.modality != null) 
                result = header.modality.TryToValueType(ModalityTypeEnum.self_reported);
            return result;
        }

        /// <summary>
        /// SPHR プロファイルを取得します。
        /// </summary>
        /// <param name="indexPhrPath"></param>
        /// <param name="importType"></param>
        /// <returns></returns>
        internal static SphrProfile GetProfile(string indexPhrPath, DocumentReferenceTypeEnum importType)
        {
            LogHelper.Write(string.Format("SphrHelper.GetProfile: {0}", indexPhrPath));

            SphrProfile result = new SphrProfile();

            result.Initialize(FileIOHelper.OpenIndexPhr(indexPhrPath));

            // DocumentReference から各種データを取得
            if (result.IndexPhr != null && result.IndexPhr.DocumentReferences != null && result.IndexPhr.DocumentReferences.Any()) {
                SphrJsonSerializer serializer = new SphrJsonSerializer();
                foreach (DocumentReference reference in result.IndexPhr.DocumentReferences) {
                    string filePath = Path.Combine(Path.GetDirectoryName(indexPhrPath)!, reference.content.First().attachment.url);
                    string json = string.Empty;

                    if (filePath != null && File.Exists(filePath)) {
                        using (StreamReader reader = new StreamReader(filePath)) {
                            json = reader.ReadToEnd();
                        }
                    } else {
                        LogHelper.Write(string.Format("file is not found.: {0}", filePath));
                    }

                    if (!string.IsNullOrWhiteSpace(json)) {
                        //LogHelper.Write(json);
                        DocumentReferenceTypeEnum type = GetResourceType(reference.type.coding.First().CodeSystem());

                        if (importType == DocumentReferenceTypeEnum.None || importType.HasFlag(type)) {
                            switch (type) {
                                case DocumentReferenceTypeEnum.BodyWeight:
                                    BodyWeight bw = serializer.Deserialize<BodyWeight>(json)!;
                                    if (bw != null) {
                                        if (result.BodyWeights == null) result.BodyWeights = new Dictionary<ModalityTypeEnum, BodyWeight>();
                                        ModalityTypeEnum modality = SphrHelper.GetModality(bw.header);
                                        if (!result.BodyWeights.ContainsKey(modality)) {
                                            result.BodyWeights.Add(modality, bw);
                                        } else {
                                            result.BodyWeights[modality] = bw;
                                        }
                                    }
                                    break;
                                case DocumentReferenceTypeEnum.BloodPressure:
                                    BloodPressure bp = serializer.Deserialize<BloodPressure>(json)!;
                                    if (bp != null) {
                                        if (result.BloodPressures == null) result.BloodPressures = new Dictionary<ModalityTypeEnum, BloodPressure>();
                                        ModalityTypeEnum modality = SphrHelper.GetModality(bp.header);
                                        if (!result.BloodPressures.ContainsKey(modality)) {
                                            result.BloodPressures.Add(modality, bp);
                                        } else {
                                            result.BloodPressures[modality] = bp;
                                        }
                                    } else { LogHelper.Write("bp is null."); }
                                    break;
                                case DocumentReferenceTypeEnum.PhysicalActivity:
                                    PhysicalActivity pa = serializer.Deserialize<PhysicalActivity>(json)!;
                                    if (pa != null) { 
                                        if (result.PhysicalActivities == null) result.PhysicalActivities = new Dictionary<ModalityTypeEnum, PhysicalActivity>();
                                        ModalityTypeEnum modality = SphrHelper.GetModality(pa.header); 
                                        if (!result.PhysicalActivities.ContainsKey(modality)) {
                                            result.PhysicalActivities.Add(modality, pa);
                                        } else {
                                            result.PhysicalActivities[modality] = pa;
                                        }
                                    } else { LogHelper.Write("pa is null."); }
                                    break;
                                case DocumentReferenceTypeEnum.BodyTemperature:
                                    BodyTemperature bt = serializer.Deserialize<BodyTemperature>(json)!;
                                    if (bt != null) {
                                        if (result.BodyTemperatures == null) result.BodyTemperatures = new Dictionary<ModalityTypeEnum, BodyTemperature>();
                                        ModalityTypeEnum modality = SphrHelper.GetModality(bt.header);
                                        if (!result.BodyTemperatures.ContainsKey(modality)) {
                                            result.BodyTemperatures.Add(modality, bt);
                                        } else {
                                            result.BodyTemperatures[modality] = bt;
                                        }
                                    }
                                    break;
                                case DocumentReferenceTypeEnum.OxygenSaturation:
                                    OxygenSaturation os = serializer.Deserialize<OxygenSaturation>(json)!;
                                    if (os != null) { 
                                        if (result.OxygenSaturations == null) result.OxygenSaturations = new Dictionary<ModalityTypeEnum, OxygenSaturation>();
                                        ModalityTypeEnum modality = SphrHelper.GetModality(os.header);
                                        if (!result.OxygenSaturations.ContainsKey(modality)) {
                                            result.OxygenSaturations.Add(modality, os);
                                        } else {
                                            result.OxygenSaturations[modality] = os;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        } else {
                            LogHelper.Write("invalid extractType.");
                        }
                    } else {
                        LogHelper.Write("json stream is null.");
                    }
                }
            } else {
                LogHelper.Write("index.phr is null.");
            }

            return result;

        }

        /// <summary>
        /// SPHR プロファイルを保存します。
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceName"></param>
        /// <param name="profile"></param>
        /// <param name="savePath"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        internal static bool SaveProfile(string serviceId, string serviceName, SphrProfile? profile, string savePath, DateTime creationDate)
        {
            bool result = false;
            IndexPhr index = new IndexPhr();

            if (profile != null) {
                SphrJsonSerializer serializer = new SphrJsonSerializer();

                // 体重
                if (profile.BodyWeights != null) {

                }

                // 歩数
                index.DocumentReferences.AddRange(
                    SphrHelper.CreateDocumentReference<PhysicalActivity, PhysicalActivityBody>(
                        serializer, savePath, DocumentReferenceTypeEnum.PhysicalActivity, 
                        profile.PhysicalActivities, creationDate
                    )
                );

                // 血圧
                index.DocumentReferences.AddRange(
                    SphrHelper.CreateDocumentReference<BloodPressure, BloodPressureBody>(
                        serializer, savePath, DocumentReferenceTypeEnum.BloodPressure, 
                        profile.BloodPressures, creationDate
                    )
                );

                // 体温
                if (profile.BodyTemperatures != null) {

                }

                // SpO2
                if (profile.OxygenSaturations != null) {

                }
            }


            // Composition
            index.Composition = BuildComposition(serviceId, serviceName, creationDate);
            // DocumentManifest
            index.DocumentManifest = BuildDocumentManifest(Guid.NewGuid(), index.DocumentReferences.ConvertAll((i) => i.id.ToValueType<Guid>()));

            result = FileIOHelper.SaveIndexPhr(index, savePath);

            return result;
        }

        /// <summary>
        /// PHRデータ項目からDocumentReferenceプロファイルを生成します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="savePath"></param>
        /// <param name="type"></param>
        /// <param name="dic"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        internal static List<DocumentReference> CreateDocumentReference<T, U>(SphrJsonSerializer serializer, string savePath, DocumentReferenceTypeEnum type, Dictionary<ModalityTypeEnum, T>? dic, DateTime creationDate) where T : OmhEntityBase<U> where U : OmhBodyEntityBase
        {
            List<DocumentReference> result = new List<DocumentReference>();
            if (dic != null) {
                foreach (KeyValuePair<ModalityTypeEnum, T> item in dic) { 
                    if (item.Value.IsValid()) {
                        // ヘッダ生成
                        item.Value.header = SphrWrapper.CreateHeader(Guid.NewGuid(), creationDate, type, item.Key);

                        Guid uuid = item.Value.header.uuid.ToValueType<Guid>();
                        string fileName = string.Format(SphrConst.SCHEMA[type].format, uuid.ToGuidString(false));
                        string filePath = Path.Combine(savePath, fileName);
                        string contentType = SphrConst.SCHEMA[type].contentType;

                        using (StreamWriter writer = new StreamWriter(filePath, false, SphrConst.ENCODING)) {
                            writer.Write(serializer.Serialize(item.Value));
                        }
                        result.Add(BuildDocumentReference(uuid, type, fileName, contentType));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// index.phr プロファイルを取得します。
        /// </summary>
        /// <param name="userDir"></param>
        /// <param name="importType"></param>
        /// <returns></returns>
        internal static List<SphrProfile> Read(string userDir, DocumentReferenceTypeEnum importType)
        {
            LogHelper.Write(string.Format("SphrHelper.Read: {0}", userDir));

            List<SphrProfile> result = new List<SphrProfile>();

            //result.Add(GetProfile(Path.Combine(importPath, "index.phr"), importType));

            // 今インポートしたのと過去にインポートされたものを巻き込む
            foreach (string dir in Directory.GetDirectories(userDir, "*", SearchOption.TopDirectoryOnly)) {
                try {
                    LogHelper.Write(string.Format("SphrHelper.Read: {0}",dir));
                    result.Add(GetProfile(Path.Combine(dir, SphrConst.INDEX_PHR), importType));
                } catch (Exception ex) { 
                    LogHelper.Write(ex.Message); 
                }
            }

            return result;
        }

        /// <summary>
        /// index.phr プロファイルを保存します。
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="profile"></param>
        /// <param name="savePath"></param>
        /// <param name="creationDate"></param>
        /// <returns></returns>
        internal static string Write(SphrLibrarySettings settings, SphrProfile? profile, string savePath, DateTime creationDate)
        {
            string result = string.Empty;

            if (profile != null) {
                // ストレージに書き込む（他サービスのインポート済みフォルダと同階層→あれば一緒にエクスポートされる）
                bool isSuccess = SaveProfile(settings.ServiceId, settings.ServiceName, profile, savePath, creationDate);

                foreach(string dir in Directory.GetDirectories(Directory.GetParent(savePath)!.FullName, "*", SearchOption.TopDirectoryOnly)) {
                    if (dir == savePath) {
                        // 現在エクスポート中のフォルダ。何もしない
                    } else if (Path.GetFileName(dir)!.Contains(settings.ServiceId)) {
                        // 古い自サービスのフォルダ。現在のフォルダが最新なので削除
                        FileIOHelper.DeleteDirectory(dir);
                    } else { 
                        // 過去にインポートされた他サービスのフォルダ。残す
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// フォルダを移動します。
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        internal static bool Move(string sourcePath, string destPath)
        {
            bool result = false;

            try {
                //if (Directory.Exists(destPath)) FileIOHelper.DeleteDirectory(destPath);
                //if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
                Directory.Move(sourcePath, destPath);
                result = true;
            } catch (Exception ex) {
                LogHelper.Write(ex.Message);
            }
            
            return result;
        }

        /// <summary>
        /// index.phrを含むフォルダを検索します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string FindIndexPhrFolder(string path)
        {
            string result = string.Empty;

            if (File.Exists(Path.Combine(path, SphrConst.INDEX_PHR))) {
                result = path;
            } else {
                // ない場合はサブフォルダを再帰検索
                foreach (string dir in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly)) { 
                    result = SphrHelper.FindIndexPhrFolder(dir);
                    if (!string.IsNullOrWhiteSpace(result)) break;
                }
            }

            return result;
        }

        /// <summary>
        /// インポート時ルートフォルダを取得します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string GetImportRoot(string path)
        {
            // index.phrが見つかったフォルダのさらに親フォルダがルートフォルダとなる
            return Directory.GetParent(SphrHelper.FindIndexPhrFolder(path))!.FullName;
        }
    }
}
