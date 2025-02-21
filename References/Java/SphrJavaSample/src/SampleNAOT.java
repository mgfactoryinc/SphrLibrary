
//import java.lang.annotation.Native;
import java.io.Console;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.text.MessageFormat;
import java.util.Base64;

import com.sun.jna.Library;
import com.sun.jna.Native;
import com.fasterxml.jackson.databind.ObjectMapper;
import sphrlibrary.entities.openmhealth.BloodPressure;
import sphrlibrary.entities.openmhealth.BloodPressureBody;
import sphrlibrary.entities.openmhealth.PhysicalActivity;
import sphrlibrary.entities.openmhealth.PhysicalActivityBody;
import sphrlibrary.entities.openmhealth.ValueUnit;

public class SampleNAOT {
	static ObjectMapper mapper = new ObjectMapper();

	public interface SphrLibrary extends Library {
		// dllのパス
		// Path dllPath = Paths.get("lib/SphrLibrary.dll");
		Path dllPath = Paths.get("lib/SphrLibraryNAOT_win-x64.dll");

		SphrLibrary INSTANCE = (SphrLibrary) Native
				.loadLibrary(Paths.get("").toAbsolutePath().resolve(dllPath).toString(), SphrLibrary.class);

		// 汎用モジュールI/F
		void Version();

		boolean Init(String settingsPath, long userId);

		boolean Init(String serviceId, String serviceName, String storageRootPath, String userId);

		boolean ImportFromPath(String sphrFilePath);

		boolean ImportFromBinary(String sphrFileBase64Binary, String sphrFileName);

		String ExtractToJson(long extractType); // ※sphr-library-java.jar未対応

		String ExtractBloodPressureJson(); // 戻り値：sphrlibrary.entities.openmhealth.BloodPressure[]のjson

		String ExtractPhysicalActivityJson(); // 戻り値：sphrlibrary.entities.openmhealth.PhysicalActivity[]のjson
	}

	private static byte[] readSphrFile(Path path) {

		byte[] result = {};

		try {
			result = Files.readAllBytes(path);
		} catch (Exception ex) {
			System.err.println(ex);
		}

		return result;

	}

	private static String encodeBase64String(byte[] value) {
		return Base64.getEncoder().encodeToString(value);
	}

	private static Path getImportPath(Console console, String[] args) {

		Path result = null; // Paths.get("").toAbsolutePath().resolve(Paths.get("sample/SPHR_com.system.service-a_20250122210232755.sphr"));

		if (args.length > 0) {
			result = Paths.get(args[0]);
		} else {
			result = Paths.get(console.readLine("importPath: "));
		}

		return result;
	}

	public static void main(String[] args) {

		Console console = System.console();

		String input = console.readLine("インポート実行->1 データ抽出実行->2 両方->3 を入力してください。");
		boolean isImport = false;
		boolean isExtract = false;
		switch (Integer.parseInt(input)) {
			case 1:
				isImport = true;
				break;
			case 2:
				isExtract = true;
				break;
			case 3:
				isImport = true;
				isExtract = true;
				break;
			default:
				break;
		}

		// 汎用モジュールのインスタンス生成
		SphrLibrary lib = SphrLibrary.INSTANCE;

		// バージョン取得（組み込み確認）
		lib.Version(); //
		System.out.println();

		// 汎用モジュールの初期化
		boolean isInit = lib.Init("com.system.service-a", "サービスA", "C:\\sphr", "1234567890");
		System.err.println(isInit);

		// json設定ファイルで初期化する場合
		// Path settingsPath = Paths.get("config/SphrLibrarySettings.json");
		// String settings =
		// Paths.get("").toAbsolutePath().resolve(settingsPath).toString();
		// System.out.println(settings);
		// System.out.println();
		// lib.Init(settings, 1234567890);

		if (isImport) {
			System.out.println("インポートを開始します。");

			Path importPath = getImportPath(console, args);

			if (importPath == null || importPath.toString() == "") {
				System.out.println("インポートするsphrファイルパスが指定されませんでした。インポートをスキップします。");
				return;
			}

			//// .sphrファイル パス指定でインポート
			// boolean isSuccess = lib.ImportFromPath(importPath.toString());

			// .sphrファイル Base64バイナリでインポート
			boolean isSuccess = lib.ImportFromBinary(
					encodeBase64String(readSphrFile(importPath)),
					importPath.toString());

			if (isSuccess) {
				System.out.println("インポート完了しました。");
			} else {
				System.out.println("インポート失敗しました。");
			}
		} else {
			System.out.println("インポートをスキップしました。");
		}

		if (isExtract) {
			System.out.println("データ抽出を開始します。");
			//
			// long type = 6; // BloodPressure(2), PhysicalActivity(4)
			// String sphrJson = lib.ExtractToJson(type);
			// System.out.println(sphrJson);

			// 血圧データの抽出
			String bpJson = lib.ExtractBloodPressureJson();
			System.err.println("<BloodPressure JsonData>");
			System.out.println(bpJson);
			System.out.println();

			// 歩数データの抽出
			// isInit = lib.Init("com.system.service-a", "サービスA", "C:\\sphr", "1234567890");
			String paJson = lib.ExtractPhysicalActivityJson();
			System.err.println("<PhysicalActivity JsonData>");
			System.out.println(paJson);
			System.out.println();

			ObjectMapper mapper = new ObjectMapper();
			BloodPressure[] bpArray = null;
			PhysicalActivity[] paArray = null;

			try {
				// クラスにデシリアライズ
				if (bpJson != null && !bpJson.isEmpty()) {
					bpArray = mapper.readValue(bpJson, BloodPressure[].class);
				}
				if (paJson != null && !paJson.isEmpty()) {
					paArray = mapper.readValue(paJson, PhysicalActivity[].class);
				}
			} catch (IOException e) {
				e.printStackTrace();
			}

			if (bpArray != null && bpArray.length > 0) {
				System.out.println("<BloodPressure ClassData>");
				for (BloodPressure bp : bpArray) { // モダリティごとの配列
					// クラスから取り込む
					if (bp != null) {
						if (bp.getHeader() != null) {
							System.out.println(MessageFormat.format("modality:{0}", bp.getHeader().getModality()));
						}
					}

					if (bp.getBody() != null) {
						for (BloodPressureBody body : bp.getBody()) {
							String recordDate = body.getEffectiveTimeFrame().getDateTime();
							ValueUnit systolic = body.getDiastolicBloodPressure();
							ValueUnit diastolic = body.getDiastolicBloodPressure();
							System.out.println(MessageFormat.format("{0}:{1}{2}-{3}{4}",
									recordDate, systolic.getValue(), systolic.getUnit(),
									diastolic.getValue(), diastolic.getUnit()));
						}
					}
				}

				System.out.println();
			} else {
				System.out.println("血圧データなし");
			}

			if (paArray != null && paArray.length > 0) {
				System.out.println("<PhysicalActivity ClassData>");
				for (PhysicalActivity pa : paArray) { // モダリティごとの配列
					// クラスから取り込む
					if (pa != null) {
						if (pa.getHeader() != null) {
							System.out.println(MessageFormat.format("modality:{0}", pa.getHeader().getModality()));
						}
					}

					if (pa.getBody() != null) {
						for (PhysicalActivityBody body : pa.getBody()) {
							// 歩数、距離は1日分(JSTタイムゾーンの00:00:00-23:59:59)の合計値を持っています
							String startDate = body.getEffectiveTimeFrame().getTimeInterval().getStartDateTime();
							String EndDate = body.getEffectiveTimeFrame().getTimeInterval().getEndDateTime();
							ValueUnit steps = body.getBaseMovementQuantity();
							ValueUnit distance = body.getDistance();
							System.out.println(MessageFormat.format("{0}-{1}:{2}{3} {4}{5}", startDate, EndDate,
									steps.getValue(), steps.getUnit(), distance.getValue(), distance.getUnit()));
						}
					}
				}

				System.out.println();
			} else {
				System.out.println("歩数データなし");
			}

			System.out.println("データ抽出が完了しました。");
		} else {
			System.out.println("データ抽出をスキップしました。");
		}

		return;
	}
}
