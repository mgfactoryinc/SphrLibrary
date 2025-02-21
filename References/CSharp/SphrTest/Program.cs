using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Entities.SPHR;
using SphrLibrary.Extensions;
using SphrTest;
using System.Reflection;
//using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine(TestWorker.GetVersion());
OperatingSystem os = Environment.OSVersion;
switch (os.Platform) {
    case PlatformID.Unix:
        break;
    case PlatformID.Win32NT:
        break;
    default:
        break;
}
Console.WriteLine(os.ToString());

//> test.exe Import,Export C:\\sphr\\ 1234567890
//> test.dll Import,Export /home/[username]/SPHR 1234567890
OperationTypeEnum operationType = args[0].TryToValueType(OperationTypeEnum.None);
string rootPath = args[1];
string serviceId = "com.service-a.company";
string serviceName = Assembly.GetExecutingAssembly().GetName().Name!;
string userId = args[2];

// コマンドライン未指定の場合はコンソール入力
if (string.IsNullOrWhiteSpace(rootPath)) {
    Console.WriteLine("rootPath:");
    rootPath = Console.ReadLine()!;
}
if (string.IsNullOrWhiteSpace(userId)) {
    Console.WriteLine("userId:");
    userId = Console.ReadLine()!;
}

Console.WriteLine("rootPath:{0}", rootPath);
Console.WriteLine("serviceId:{0}", serviceId);
Console.WriteLine("serviceName:{0}", serviceName);
Console.WriteLine("userId:{0}", userId);

////// ここからエクスポート //////

// 汎用モジュール初期化
SphrLibrary.SphrLibrary.Init(serviceId, serviceName, rootPath, userId);

// test
// string test = SphrUnmanagedLibrary.Test();
// Console.WriteLine(test);

string exportPath = string.Empty;

if (operationType.HasFlag(OperationTypeEnum.Export)) {
    Console.WriteLine("エクスポートを実行します。");

    // エクスポート
    bool exportResult = TestWorker.Export(ref exportPath);

    if (exportResult && !string.IsNullOrWhiteSpace(exportPath)) {
        Console.WriteLine("エクスポートが完了しました。: {0}", exportPath);
    } else {
        // 実運用でエラーになるケースは実データに問題あり。格納方法などのコード上のエラーはテスト段階で潰しきっておく
        if (SphrLibrary.SphrLibrary.Errors != null && SphrLibrary.SphrLibrary.Errors.Any()) { 
            // エラー処理
            foreach (SphrResult item in  SphrLibrary.SphrLibrary.Errors) { 
                Console.WriteLine("{0}:{1}", item.code, item.detail);
            }
        }
        Console.WriteLine("エクスポートに失敗しました。エラー内容を確認してください。");
    }
}

////// ここからインポート //////
serviceId = "jp.co.mgfactory";
serviceName = Assembly.GetExecutingAssembly().GetName().Name!;
//userId = args[2];

// 汎用モジュール初期化
// インポートとエクスポートは本来独立しているはずなのでそれぞれの処理を想定したサンプルにしています。
// 一連処理でやる場合は初期化1回でいいです
SphrLibrary.SphrLibrary.Init(serviceId, serviceName, rootPath, userId);

string importPath = string.Empty;

if (operationType.HasFlag(OperationTypeEnum.Import)) {
    Console.WriteLine("インポートを実行します。");

    // インポートだけ実行する場合は.sphrファイルパス指定
    if (string.IsNullOrWhiteSpace(exportPath)) {
        Console.WriteLine("importFilePath(*.sphr):");
        importPath = Console.ReadLine()!;
    } else {
        importPath = exportPath;
    }

    if (File.Exists(importPath)) {
        // インポート実行（抽出項目をビットフラグで指定）
        bool importResult = TestWorker.Import(importPath);
    } else {
        Console.WriteLine("SPHRファイルが見つかりませんでした。");
    }
}

////// ここからデータ抽出 //////

if (operationType.HasFlag(OperationTypeEnum.Extract)) {
    Console.WriteLine("データ抽出を実行します。");
    SphrProfile? profile = TestWorker.Extract();

    if (profile != null && profile.PhysicalActivities != null) {
        // 歩数を取り出す
        List<PhysicalActivity> paList = profile.PhysicalActivities.Values.ToList();
        if (paList != null && paList.Any()) { 
            foreach (PhysicalActivity pa in paList) {
                if (pa != null) {
                    Console.WriteLine("[PhysicalActivity]");
                    if (pa.header != null) Console.WriteLine("modality: {0}", pa.header.modality);
                    if (pa.body != null && pa.body.Any()) {
                        // 自サービスに取り込みます（ここではコンソール出力）
                        foreach (PhysicalActivityBody body in pa.body) {
                            Console.WriteLine("{0}:{1}{2}/{3}{4} ({5})", 
                                body.effective_time_frame.time_interval!.start_date_time.TryToValueType<DateTime>(DateTime.MinValue).ToString("yyyy/MM/dd"),
                                body.base_movement_quantity.value.ToString(), 
                                body.base_movement_quantity.unit,
                                body.distance.value.ToString(),
                                body.distance.unit,
                                body.activity_name
                            );
                        }
                    } else { 
                        // 歩数データ無し（正常）
                    }
                }
            }
        }
        
    } else { 
        // 抽出設定なし（正常）
    }

    if (profile != null && profile.BloodPressures != null) {
        // 血圧の取り込み
        List<BloodPressure> bpList = profile.BloodPressures.Values.ToList();
        if (bpList != null && bpList.Any()) {
            foreach (BloodPressure bp in bpList) {
                if (bp != null) {
                    Console.WriteLine("[BloodPressure]");
                    if (bp.header != null) Console.WriteLine("modality: {0}", bp.header.modality);
                    if (bp.body != null && bp.body.Length > 0) {
                        foreach (BloodPressureBody body in bp.body) {
                            Console.WriteLine("{0}:{1}{2}/{3}{4} ({5}/{6}/{7})",
                                body.effective_time_frame.date_time!.TryToValueType<DateTime>(DateTime.MinValue).ToString("yyyy/MM/dd"),
                                body.systolic_blood_pressure.value.ToString(),
                                body.systolic_blood_pressure.unit,
                                body.diastolic_blood_pressure.value.ToString(),
                                body.diastolic_blood_pressure.unit,
                                body.body_posture,
                                body.measurement_location,
                                body.temporal_relationship_to_physical_activity
                            );
                        }
                    }
                }
            }
        }
        
    }

    Console.WriteLine("データ抽出が完了しました。");
}