using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

//namespace SphrLibrary;

/// <summary>
/// SPHR 汎用モジュール(アンマネージド)に関する機能を提供します。
/// </summary>
public static class SphrUnmanagedLibrary
{
    //EntryPointの名前は同名不可です（ビルド時に警告出ますが、Windowsは問題無し、Linuxはエラーになる）
    //オーバロード不可と思っておいた方が良さそう

    /// <summary>
    /// バージョン情報を取得します。
    /// </summary>
    [UnmanagedCallersOnly(EntryPoint = "Version")]
    public static void Version()
    {
        try {
            Console.WriteLine(SphrHelper.GetVersion());
        } catch {}
    }

    /// <summary>
    /// 対象ユーザーIDを初期化します。
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "InitUser")]
    public static bool InitUser(Int64 userId) {
        bool result = false;
        try {
            SphrLibrary.SphrLibrary.Init(
                Path.Combine(FileIOHelper.DefaultPath(Environment.OSVersion), SphrConst.SETTINGS_FILE_NAME), userId);
            result = true;
        } catch (Exception ex) { 
            Console.WriteLine(ex.Message);
        }
        return result;
    }

    /// <summary>
    /// 設定ファイルパスを指定して、汎用モジュールを初期化します。
    /// </summary>
    /// <param name="settingsPath"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "InitWithSettings")]
    public static bool InitWithSettings(IntPtr settingsPath, Int64 userId) {
        bool result = false;
        try {
            string? path = Marshal.PtrToStringAnsi(settingsPath);
            //Console.WriteLine("settingsPath:{0}", path);
            if (!String.IsNullOrWhiteSpace(path)) {
                SphrLibrary.SphrLibrary.Init(path, userId);
                result = true;
            }
        } catch (Exception ex) { 
            Console.WriteLine(ex.Message);
        }
        return result;
    }

    /// <summary>
    /// 設定情報を指定して、汎用モジュールを初期化します。
    /// </summary>
    /// <param name="serviceId"></param>
    /// <param name="serviceName"></param>
    /// <param name="storageRootPath"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "Init")]
    public static bool Init(IntPtr serviceId, IntPtr serviceName, IntPtr storageRootPath, IntPtr userId) {
        bool result = false;

        try {
            string? id = Marshal.PtrToStringAnsi(serviceId);
            string? name = Marshal.PtrToStringAnsi(serviceName);
            string? path = Marshal.PtrToStringAnsi(storageRootPath);
            string? uid = Marshal.PtrToStringAnsi(userId);

            if (!string.IsNullOrWhiteSpace(id)
                && !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(path)
                && !string.IsNullOrWhiteSpace(uid)) {
                SphrLibrary.SphrLibrary.Init(id, name, path, uid);
                LogHelper.Write(string.Format("{0},{1},{2},{3}", id, name, path, uid));
                result = true;
            }
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
        
        return result; 
    }

    /// <summary>
    /// エクスポート用 血圧データ(サンプル)を格納します。
    /// </summary>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "SetBloodPressureSample")]
    public static bool SetBloodPressureSample()
    {
        try {
            LogHelper.Write("SetBloodPressureSample");
            BloodPressure bp = SphrWrapper.CreateBloodPressure(
                new DateTime(2024, 12, 1), ModalityTypeEnum.self_reported, 144, 82, 
                BodyPostureTypeEnum.sitting, BloodPressureMeasurementLocationTypeEnum.rightupperarm, 
                TemporalRelationshipToPhysicalActivityTypeEnum.at_rest, DateTime.Now
            );
            LogHelper.Write(new SphrJsonSerializer().Serialize<BloodPressure>(bp));
            // sample data 
            return SphrLibrary.SphrLibrary.Set(bp);
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }

        return false;
    }

    /// <summary>
    /// エクスポート用 歩数データ(サンプル)を格納します。
    /// </summary>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "SetPhysicalActivitySample")]
    public static bool SetPhysicalActivitySample()
    {
        try {
            LogHelper.Write("SetPhysicalActivitySample");
            PhysicalActivity pa = SphrWrapper.CreatePhysicalActivity(new DateTime(2024, 12, 1), ModalityTypeEnum.self_reported, "Walking", 5000, 1.8, DateTime.Now);
            LogHelper.Write(new SphrJsonSerializer().Serialize<PhysicalActivity>(pa));
            return SphrLibrary.SphrLibrary.Set(pa);
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return false;
    }

    /// <summary>
    /// エクスポート用 血圧データを格納します。
    /// </summary>
    /// <param name="recordDate"></param>
    /// <param name="modality"></param>
    /// <param name="systolic"></param>
    /// <param name="diastolic"></param>
    /// <param name="bodyPosture"></param>
    /// <param name="measurementLocation"></param>
    /// <param name="temporalRelationship"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "SetBloodPressure")]
    public static bool SetBloodPressure(Int64 recordDate, int modality, int systolic, int diastolic, 
        int? bodyPosture, int? measurementLocation, int? temporalRelationship) {

        try {
            LogHelper.Write("SetBloodPressure");

            ModalityTypeEnum modalityType = ModalityTypeEnum.self_reported;
            BodyPostureTypeEnum? bodyPostureType = null;
            BloodPressureMeasurementLocationTypeEnum? measurementType = null;
            TemporalRelationshipToPhysicalActivityTypeEnum? temporalRelationshipType = null;

            switch (modality) {
                case (int)ModalityTypeEnum.sensed:
                case (int)ModalityTypeEnum.self_reported:
                    modalityType = (ModalityTypeEnum)Enum.ToObject(typeof(ModalityTypeEnum), modality);
                    break;
            }
            if (bodyPosture.HasValue) {
                if ((int)BodyPostureTypeEnum.sitting <= bodyPosture.Value && bodyPosture.Value <= (int)BodyPostureTypeEnum.semi_recumbent) {
                    bodyPostureType = (BodyPostureTypeEnum)Enum.ToObject(typeof(BodyPostureTypeEnum), bodyPosture.Value);
                }
            }
            if (measurementLocation.HasValue) {
                if ((int)BloodPressureMeasurementLocationTypeEnum.leftankle <= measurementLocation.Value && measurementLocation.Value <= (int)BloodPressureMeasurementLocationTypeEnum.rightwrist) {
                    measurementType = (BloodPressureMeasurementLocationTypeEnum)Enum.ToObject(typeof(BloodPressureMeasurementLocationTypeEnum), measurementLocation.Value);
                }
            }
            if (temporalRelationship.HasValue) {
                if ((int)TemporalRelationshipToPhysicalActivityTypeEnum.at_rest <= temporalRelationship.Value && temporalRelationship.Value <= (int)TemporalRelationshipToPhysicalActivityTypeEnum.during_exercise) {
                    temporalRelationshipType = (TemporalRelationshipToPhysicalActivityTypeEnum)Enum.ToObject(typeof(TemporalRelationshipToPhysicalActivityTypeEnum), temporalRelationship.Value);
                }
            }
            BloodPressure bp = SphrWrapper.CreateBloodPressure(
                new DateTime(recordDate), modalityType, systolic, diastolic, bodyPostureType, measurementType, temporalRelationshipType, DateTime.Now);
            LogHelper.Write(new SphrJsonSerializer().Serialize<BloodPressure>(bp));
            // sample data 
            return SphrLibrary.SphrLibrary.Set(bp);
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        } 
        
        return false;
    }

    /// <summary>
    /// エクスポート用 歩数データを格納します。
    /// </summary>
    /// <param name="recordDate"></param>
    /// <param name="modality"></param>
    /// <param name="steps"></param>
    /// <param name="distanceKm"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "SetPhysicalActivity")]
    public static bool SetPhysicalActivity(Int64 recordDate, int modality, double steps, double distanceKm) {
        
        try {
            LogHelper.Write("SetPhysicalActivity");

            ModalityTypeEnum modalityType = ModalityTypeEnum.self_reported;
            switch (modality) {
                case (int)ModalityTypeEnum.sensed:
                case (int)ModalityTypeEnum.self_reported:
                    modalityType = (ModalityTypeEnum)Enum.ToObject(typeof(ModalityTypeEnum), modality);
                    break;
            }
            PhysicalActivity pa = SphrWrapper.CreatePhysicalActivity(new DateTime(recordDate), modalityType, "Walking", steps, distanceKm, DateTime.Now);
            LogHelper.Write(new SphrJsonSerializer().Serialize<PhysicalActivity>(pa));
            return SphrLibrary.SphrLibrary.Set(pa);
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return false;
    }

    /// <summary>
    /// エクスポートを実行します。
    /// </summary>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "Export")]
    public static Int64 Export()
    {
        Int64 result = -1;

        try {
            DateTime exportDate = DateTime.Now;
            string exportPath = SphrLibrary.SphrLibrary.Export(exportDate);
            if (!string.IsNullOrWhiteSpace(exportPath)) {
                // ミリ秒以下7桁->3桁にする
                Int64 ticks = exportDate.Ticks / 10000; //exportDate.ToUniversalTime().Ticks / 10000;
                Int64 preset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
                // LogHelper.Write(string.Format("{0} - {1} = {2}", ticks, preset, ticks - preset));

                result = ticks - preset;
            } else {
                LogHelper.Write("export path is null.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return result;
    }

    // [UnmanagedCallersOnly(EntryPoint = "ExportByCreationDate")]
    // public static Int64 ExportByCreationDate(Int64 sourceCreationDateTime)
    // {
    //     Int64 result = -1;
    //     DateTime exportDate = Convert.ToDateTime(sourceCreationDateTime);
    //     if (!string.IsNullOrWhiteSpace(SphrLibrary.SphrLibrary.Export(exportDate))) {
    //         result = exportDate.Ticks;
    //     } else { 
        
    //     }
    //     return result;

        ////string path = SphrLibrary.Export();
        
        ////Console.WriteLine(SphrLibrary.SphrLibrary.Profile.StorageRootPath);
        ////Console.WriteLine("bp.count:{0}", SphrLibrary.SphrLibrary.Profile.BloodPressures!.Count);
        ////Console.WriteLine("pa.count:{0}", SphrLibrary.SphrLibrary.Profile.PhysicalActivities!.Count);
        //Int64 result = 0;
        //SphrExportWorkerResults results = new SphrExportWorker().TryExecute(new() { Profile = SphrLibrary.SphrLibrary.Profile, ExportDate = exportDate });
        ////Console.WriteLine(results.ToString());

        //if (results != null) {
        //    //Console.WriteLine(results.IsSuccess);
        //    if (results.IsSuccess) {
        //        LogHelper.Write(exportDate.ToDateString());
        //        // ミリ秒以下7桁->3桁にする
        //        Int64 ticks = exportDate.ToUniversalTime().Ticks / 10000;
        //        Int64 preset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
        //        LogHelper.Write(string.Format("{0} - {1} = {2}", ticks, preset, ticks - preset));
                
        //        result = ticks - preset;
        //        //result = exportDate.ToString("yyyyMMddHHmmssfff").ToValueType<Int64>();
        //        //result = results.ExportFilePath;
        //    } else if (results.Results != null && results.Results.Any()) {                    
        //        SphrLibrary.SphrLibrary.Errors = new List<SphrResult>();
        //        SphrLibrary.SphrLibrary.Errors.AddRange(results.Results);
        //        results.Results.ForEach((x) => LogHelper.Write(string.Format("{0}:{1}", x.code, x.detail)));
        //    } else { 
                
        //    }
        //} else { 
        
        //}
        //return result;

    //[UnmanagedCallersOnly(EntryPoint = "ExportByCreationDate")]
    //public static IntPtr ExportByCreationDate(IntPtr sourceCreationDateTime)
    //{
    //    try {
    //        DateTime exportDate = DateTime.Now;
    //        string? creation = Marshal.PtrToStringAnsi(sourceCreationDateTime);
    //        if (!string.IsNullOrWhiteSpace(creation)) {
    //            DateTime temp = creation.TryToValueType(DateTime.MinValue);
    //            if (temp != DateTime.MinValue) exportDate = temp;
    //        }

    //        string exportPath = SphrLibrary.SphrLibrary.Export(exportDate);

    //        if (!string.IsNullOrWhiteSpace(exportPath)) {
    //            return Marshal.StringToHGlobalAnsi(exportPath);
    //        } else {
    //            LogHelper.Write("export path is null.");
    //        }
    //    } catch (Exception ex) {
    //        LogHelper.Write(ex.Message);
    //    }

    //    return IntPtr.Zero;
    //}

    /// <summary>
    /// ファイルパスを指定して、インポートを実行します。
    /// </summary>
    /// <param name="sphrFilePath"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "ImportFromPath")]
    public static bool ImportFromPath(IntPtr sphrFilePath)
    {
        bool result = false;
        try {
            string? filePath = Marshal.PtrToStringAnsi(sphrFilePath);
            LogHelper.Write(string.Format("sphrFilePath: {0}", filePath));

            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath)) {
                result = SphrLibrary.SphrLibrary.Import(filePath);
            } else {
                LogHelper.Write("invalid path.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return result;
    }

    /// <summary>
    /// バイナリデータを指定して、インポートを実行します。
    /// </summary>
    /// <param name="sphrFileBase64Binary"></param>
    /// <param name="sphrFileName"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "ImportFromBinary")]
    public static bool ImportFromBinary(IntPtr sphrFileBase64Binary, IntPtr sphrFileName) {

        bool result = false;
        try {
            string? base64binary = Marshal.PtrToStringAnsi(sphrFileBase64Binary);
            if (!string.IsNullOrWhiteSpace(base64binary)) {
                LogHelper.Write(string.Format("ImportFromBinary base64binary.Length: {0}", base64binary.Length));
                byte[] binary = Convert.FromBase64String(base64binary);
                string? fileName = Marshal.PtrToStringAnsi(sphrFileName);
                result = SphrLibrary.SphrLibrary.Import(binary, fileName!);
            } else {
                LogHelper.Write("base64binary is null.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return result;
    }

    /// <summary>
    /// データ抽出を実行し、SPHRプロファイル情報をJson形式で取得します。
    /// </summary>
    /// <param name="extractType"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "ExtractToJson")]
    public static IntPtr ExtractToJson(UInt64 extractType) {

        try
        {
            DocumentReferenceTypeEnum type = (DocumentReferenceTypeEnum)Enum.ToObject(typeof(DocumentReferenceTypeEnum), extractType);
            SphrLibrary.SphrLibrary.Profile = SphrLibrary.SphrLibrary.Extract(type);

            if (SphrLibrary.SphrLibrary.Profile != null) {
                string? json = new SphrJsonSerializer().Serialize<SphrProfile>(SphrLibrary.SphrLibrary.Profile!);
                LogHelper.Write(json);
                // SphrProfile型のJson
                return Marshal.StringToHGlobalAnsi(json);
            } else {
                LogHelper.Write("profile is null.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return IntPtr.Zero;
    }

    /// <summary>
    /// データ抽出を実行し、SPHRプロファイル情報をクラス形式で取得します。
    /// </summary>
    /// <param name="extractType"></param>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "ExtractToClass")]
    [RequiresDynamicCode("")]
    public static IntPtr ExtractToClass(UInt64 extractType) {
        DocumentReferenceTypeEnum type = (DocumentReferenceTypeEnum)Enum.ToObject(typeof(DocumentReferenceTypeEnum), extractType);
        SphrLibrary.SphrLibrary.Profile = SphrLibrary.SphrLibrary.Extract(type);

        // SphrProfileクラス
        return SphrHelper.ToPtr(SphrLibrary.SphrLibrary.Profile!);
    }

    /// <summary>
    /// データ抽出を実行し、血圧データをJson形式で取得します。
    /// </summary>
    /// <returns></returns>
    [UnmanagedCallersOnly(EntryPoint = "ExtractBloodPressureJson")]
    public static IntPtr ExtractBloodPressureJson() {
        try {
            SphrLibrary.SphrLibrary.Profile = SphrLibrary.SphrLibrary.Extract(DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity);

            if (SphrLibrary.SphrLibrary.Profile != null && SphrLibrary.SphrLibrary.Profile.BloodPressures != null) {
                string? json = new SphrJsonSerializer().Serialize<BloodPressure[]>(SphrLibrary.SphrLibrary.Profile.BloodPressures.Values.ToArray());
                LogHelper.Write(json);
                // BloodPressureクラスのJson
                return Marshal.StringToHGlobalAnsi(json);
            } else {
                LogHelper.Write("profile is null.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }
        
        return IntPtr.Zero;
    }

    [UnmanagedCallersOnly(EntryPoint = "ExtractBloodPressureClass")]
    [RequiresDynamicCode("")]
    public static IntPtr ExtractBloodPressureClass() {

        try {
            SphrLibrary.SphrLibrary.Profile = SphrLibrary.SphrLibrary.Extract(DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity);
        
            if (SphrLibrary.SphrLibrary.Profile != null && SphrLibrary.SphrLibrary.Profile.BloodPressures != null) {
                // BloodPressureクラス
                return SphrHelper.ToPtr(SphrLibrary.SphrLibrary.Profile!.BloodPressures);
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }

        return IntPtr.Zero;
    }

    [UnmanagedCallersOnly(EntryPoint = "ExtractPhysicalActivityJson")]
    public static IntPtr ExtractPhysicalActivityJson() {

        try {
            SphrLibrary.SphrLibrary.Profile = SphrLibrary.SphrLibrary.Extract(DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity);

            if (SphrLibrary.SphrLibrary.Profile != null && SphrLibrary.SphrLibrary.Profile.PhysicalActivities != null) {
                string? json = new SphrJsonSerializer().Serialize<PhysicalActivity[]>(SphrLibrary.SphrLibrary.Profile.PhysicalActivities.Values.ToArray());
                LogHelper.Write(json);
                // PhysicalActivityクラスのJson
                return Marshal.StringToHGlobalAnsi(json);
            } else {
                LogHelper.Write("profile is null.");
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }

        return IntPtr.Zero;
    }

    [UnmanagedCallersOnly(EntryPoint = "ExtractPhysicalActivityClass")]
    [RequiresDynamicCode("")]
    public static IntPtr ExtractPhysicalActivityClass() {
        try { 
            SphrLibrary.SphrLibrary.Extract(DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity);

            if (SphrLibrary.SphrLibrary.Profile != null && SphrLibrary.SphrLibrary.Profile.PhysicalActivities != null) {
                // PhysicalActivityクラス
                return SphrHelper.ToPtr(SphrLibrary.SphrLibrary.Profile!.PhysicalActivities);
            }
        } catch (Exception ex) {
            LogHelper.Write(ex.Message);
        }

        return IntPtr.Zero;
    }

    //public static string Test() {
    //    string result = string.Empty;
    //    try {
    //        SphrLibrary.SphrLibrary.Extract(DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity);

    //        if (SphrLibrary.SphrLibrary.Profile != null && SphrLibrary.SphrLibrary.Profile.PhysicalActivities != null) {
    //            string? json = new SphrJsonSerializer().Serialize<PhysicalActivity[]>(SphrLibrary.SphrLibrary.Profile.PhysicalActivities.Values.ToArray());
    //            LogHelper.Write(json);
    //            // PhysicalActivityクラスのJson
    //            return json;
    //        } else {
    //            LogHelper.Write("profile is null.");
    //        }
    //    } catch (Exception ex) {
    //        LogHelper.Write(ex.Message);
    //    }

    //    return result;
    //}
}
