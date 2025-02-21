# インポートを行う事業者さま向け組み込み手順
> [!NOTE]
> 汎用モジュールはインポート・エクスポート双方を1つの実行ファイルで行いますが、この手順書ではJavaを利用したインポート処理について重点的に解説します。

## 想定する言語・環境
<img src="https://img.shields.io/badge/Java SE-8.0以上-000.svg?style=for-the-badge">　<img src="https://img.shields.io/badge/Windows-10以上-000.svg?style=for-the-badge">

## #1 ビルド・動作検証

<a href="https://github.com/mgfactoryinc/SphrLibrary/tree/master/References/Java/SphrJavaSample">SphrJavaSample</a><br>
1. 上記プロジェクトファイルをIDEにてビルド
   - GradleやMaven等のパッケージ管理ツールにて、以下ライブラリのインストールが必要な場合があります。
		- jna-5.13.0.jar
		- jna-platform-5.13.0.jar
		- jackson-core-2.18.2.jar
		- jackson-databind-2.18.2.jar
		- jackson-annotations-2.18.2.jar
2. ビルドされた対話式コンソールアプリ（サンプル）を実行
3. JSONの取得成功まで検証

## #2 ディレクトリの用意
Rootディレクトリ直下にsphr（大文字・小文字利用可能）ディレクトリを作成し、書込み権限のアタッチを行ってください。

【参考】汎用モジュールが利用（作成）するディレクトリ構成<br>
（以下 temp , sphrdb , log 各フォルダが自動生成されない場合、手動で追加を行って下さい）
```
例：ユーザーIDが1234567890の場合
[root]
├temp（生成したファイルを格納するディレクトリ。エクスポート時に生成）
├sphrdb（過去にインポートしたファイルを管理するディレクトリ）
　　└1234
　　　└5678
　　　　└1234567890
　　　　　├SPHR_service-b_yyyymmddhhmmssfff　←過去にインポート済（非圧縮）
　　　　　└SPHR_service-c_yyyymmddhhmmssfff　←過去にインポート済（非圧縮）
└log（ログファイルを格納するディレクトリ）
```


## #3 実行
<a href="https://github.com/mgfactoryinc/SphrLibrary/blob/master/References/Java/SphrJavaSample/src/SampleNAOT.java">Java向け呼び出しサンプルコード</a>を参考に以下の方法で汎用モジュールに値を渡します。<br>


1. 汎用モジュールへSPHRファイルを受け渡し
   - ファイルパス or バイナリで受け渡し。受け渡し可能なファイルであれば戻り値でTrueが返ります。
2. データ抽出
   - インポートしたい項目の関数を利用し、データを抽出

```java
public interface SphrLibrary extends Library {
	// dllのパス
	// Path dllPath = Paths.get("lib/SphrLibrary.dll");
	Path dllPath = Paths.get("lib/SphrLibraryNAOT_win-x64.dll");

	SphrLibrary INSTANCE = (SphrLibrary) Native
			.loadLibrary(Paths.get("").toAbsolutePath().resolve(dllPath).toString(), SphrLibrary.class);

	// 汎用モジュールI/F
	void Version();	
	boolean Init(String serviceId, String serviceName, String storageRootPath, String userId);
	boolean ImportFromPath(String sphrFilePath);
	boolean ImportFromBinary(String sphrFileBase64Binary, String sphrFileName);
	String ExtractToJson(long extractType); // ※sphr-library-java.jar未対応
	String ExtractBloodPressureJson(); // 戻り値：sphrlibrary.entities.openmhealth.BloodPressure[]のjson
	String ExtractPhysicalActivityJson(); // 戻り値：sphrlibrary.entities.openmhealth.PhysicalActivity[]のjson
}
```



## #4 I/F


1. 初期化<br>
関数：Init<br>
*引数*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|    | 物理名 | 論理名 | 型 | 桁数 | 〇=必須/●=非必須 | 　   |
| 1 | serviceId | サービスID | String |  　  | 〇 | サービスドメインの逆順。iOS,AndroidアプリのBundleIDと同じ要領で記載 |
| 2 | serviceName | サービス名 | String |  　  | 〇 | 　   |
| 3 | storageRootPath | ルートパス | String |  　  | 〇 |   |
| 4 | userId | ユーザーID | String |  　  | 〇 | ユーザーIDが数値の場合、最大桁数からゼロ埋め |

--------

2. 汎用モジュールへSPHRファイルを受け渡し
### ファイルパスを利用してSPHRファイルを渡す場合

関数：ImportFromPath<br>
*引数*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|    | 物理名 | 論理名 | 型 | 桁数 | 〇=必須/●=非必須 | 　   |
| 1 | sphrFilePath | インポートファイルパス | String |  　  | 〇 | 　   |

*戻り値*<br>
boolean
> [!NOTE]
> 成功した場合、汎用モジュールが管理するストレージエリアに最新版のJSONが格納されます

### バイナリとしてSPHRファイルを渡す場合
関数：ImportFromBinary<br>
*引数*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|    | 物理名 | 論理名     | 型     | 桁数 | 〇=必須/●=非必須 | 　   |
| 1 | sphrFileBase64Binary | インポートファイルバイナリ | String |  　  |〇|base64にて受け渡し|
| 2 | sphrFileName | ファイル名 | String |  　  |〇| 　   |

*戻り値*<br>
boolean
> [!NOTE]
> 成功した場合、汎用モジュールが管理するストレージエリアに最新版のJSONが格納されます

--------

3. データ抽出
> [!NOTE]
> Import関数にて汎用モジュールへ受け渡したSPHRファイルを一連の流れで抽出する関数ではなく、最新のファイルからデータを抽出するための関数となります。（Import関数でfalseが返った場合、意図しない結果が返却される可能性があります。）

血圧抽出のための関数：ExtractBloodPressureJson<br>
*引数*<br>
不要

*戻り値*<br>
血圧クラス(配列)のjson

歩数抽出のための関数：ExtractPhysicalActivityJson<br>
*引数*<br>
不要

*戻り値*<br>
歩数クラス(配列)のjson



## #5 エラー定義

### エラーコード

エラーコードとエラー内容はログに出力されます。<br>
I/Fの戻り値としてはboolean(true/false)を返します。<br>
処理結果がfalseの場合の原因調査に使用できます。

エラーコードの形式は以下のように構成されます。
```
[Level].sphr.[Method].[ErrorCode]
```

#### Level

エラーの情報レベルを判別します。<br>

| level | 名称 | 説明 |
|---|---|---|
| i | Information | 正常終了 |
| w | Warning | 警告(処理続行OK) |
| e | Error | 各種エラー |


#### Method
どの処理でエラーが発生したかを判別します。<br>

|method|名称|
|---|---|
|export|エクスポート|
|import|インポート|
|extract|データ抽出|

#### エラーコード一覧
<a href="https://github.com/mgfactoryinc/SphrLibrary/blob/master/Documents/errorcode.md">エラーコード一覧</a>を参照して下さい


