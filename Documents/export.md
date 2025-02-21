# エクスポートを行う事業者さま向け組み込み手順

> [!NOTE]
>汎用モジュールはインポート・エクスポート双方を1つの実行ファイルで行いますが、この手順書ではPHPを利用したインポート処理について重点的に解説します。

## 想定する言語・環境
<img src="https://img.shields.io/badge/PHP-8.0以上-000.svg?style=for-the-badge">　<img src="https://img.shields.io/badge/CentOS-8.0以上-000.svg?style=for-the-badge">

## #1 FFIの有効化
INI設定でFFIを有効化することで利用が可能になります。<br><br>
【参考】https://www.php.net/manual/ja/ffi.configuration.php<br>
（デフォルト"preload"を"true"と書き換える）<br>

【FFI利用方法参考】https://www.php.net/manual/ja/ffi.cdef.php<br>


## #2ディレクトリの用意
Rootディレクトリ直下にsphr（大文字・小文字利用可能）ディレクトリを作成し、書込み権限のアタッチを行ってください。

【参考】汎用モジュールが利用（作成）するディレクトリ構成<br>
（以下 temp , sphrdb , log 各フォルダが自動生成されない場合、手動で追加を行って下さい）
```
例：ユーザーIDが1234567890の場合
[root]
├temp（生成したファイルを格納するディレクトリ）
　　└1234567890　
　　　└SPHR_service-a_yyyymmddhhmmssfff.sphr　←今回、ID1234567890にてエクスポートしたsphrファイル
├sphrdb（過去にインポートしたファイルを管理するディレクトリ）
　　└1234
　　　└5678
　　　　└1234567890
　　　　　├SPHR_service-b_yyyymmddhhmmssfff　←過去にインポート済（非圧縮）
　　　　　└SPHR_service-c_yyyymmddhhmmssfff　←過去にインポート済（非圧縮）
└log（ログファイルを格納するディレクトリ）
```
## #3汎用モジュール配置、設定ファイル作成
汎用モジュールの設定ファイルをSPHRフォルダに設置します。（ファイル名はSphrLibrarySettings.json）
例：/sphr/SphrLibrarySettings.json

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|  | 物理名  | 論理名 | 型 | 桁数 | 〇=必須/●=非必須 | 汎用モジュールでエクスポート時に毎回利用する設定を保存するJSON   |
|  1 | ServiceId   | サービスID | string || 〇 | サービスドメインの逆順。iOS,AndroidアプリのBundleIDと同じ要領で記載|
|  2 | ServiceName | サービス名 | string |    | 〇 | サービス名   |
|  3 | StorageRootPath | ルートパス | string |    | 〇 | 汎用モジュールが作業するフォルダのルートパス（実行ユーザーでのアクセス権が必要） |
|  4 | UserIdDigits| ユーザーID最大桁数 | int|    | 〇 | サービサーで管理されている利用者を一意に識別するIDの最大桁数（0詰めするため）|


作成例：
``` JSON
{
	"ServiceId":"com.system.service-a",
	"ServiceName":"サービスA",
	"StorageRootPath":"/sphr",
	"UserIdDigits":"12"
}
```

## #5 実行
以下を参考に汎用モジュールに値を渡します。<br>

```PHP
<?php

// FFI共有ライブラリを動的に読み込む
$ffi = FFI::cdef('
void Version();
bool Init(int userId);
bool SetBloodPressureSample();
bool SetPhysicalActivitySample();
bool SetBloodPressure(long long int recordDate, int modality, int systolic, int diastolic, int bodyPosture, int measurementLocation, int temporalRelationship);
bool SetPhysicalActivity(long long int recordDate, int modality, float steps, float distanceKm);
long long int Export();
', __DIR__ . '/lib/SphrLibraryNAOT_linux-x64.so'); //document_rootの下にlibフォルダを切って.soファイルを置く例

// モジュールの読み込み確認
//print $ffi->Version()."\n";
$ffi->Version();

// モジュールの初期化（サービサーの利用者を一意に識別するIDを指定。数字のみ）
$ffi->Init(1234567890);

// 血圧データのセット（サンプル / SetBloodPressureSample()については/SphrLibrary/SphrUnmanagedLibrary.csを参照して下さい）
$bp_result = $ffi->SetBloodPressureSample();
print "*** blood pressure result:".$bp_result."\n\n";

// 歩数データのセット（サンプル / SetPhysicalActivitySample()については/SphrLibrary/SphrUnmanagedLibrary.csを参照して下さい）
$pa_result =  $ffi->SetPhysicalActivitySample();
print "*** physical activity result:".$pa_result."\n\n";

// エクスポート タイムスタンプ(ミリ秒3桁)が返る
// [SPHR_ROOT]/temp/SPHR_[SERVICE_ID]_YYYYMMDDHHMMSSFFF.sphr
// []内は設定ファイルの値
$timestamp = $ffi->Export();
print "*** timestamp:".$timestamp."\n\n";

// タイムスタンプを日時に変換、エクスポートされた.sphrファイルのパスを構成する
// ファイルをhttp response(application/zip)で返却
$date_string = date('Y/m/d H:i:s.v', $timestamp);
print "*** export date:".$date_string."\n\n";

return 0;
?>
	
```

### CPU実行エラー

.soファイルの実行が出来なかった場合、（CPU命令セットがサポートされていない旨のエラーが発生する場合）ご利用の環境下で汎用モジュールのビルドが必要です<br>
<a href="https://github.com/mgfactoryinc/SphrLibrary/blob/master/Documents/php-build.md">SPHR汎用モジュールのビルド手順</a>を参照して下さい


### インターフェース
### 歩数
*引数*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|  | 物理名 | 論理名   | 型  | 桁数 | 〇=必須/●=非必須 | 　  |
| 1 | recordDate | 測定日時 | long   long int |    | 〇 | タイムスタンプとして連携。時分を込めても無視されます（1日単位で丸める） |
| 2 | modality   | 測定方法 | int |    | 〇 | 0:sensed　1:self-reported   |
| 3 | steps  | 歩数 | float   |    | 〇 | 1日の合計値 |
| 4 | distanceKm | 距離 | float   |    | 〇 | 1日の合計値。連携しない場合は-1を設定   |


*戻り値*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|  | 物理名 | 論理名   | 型  | 桁数 | 〇=必須/●=非必須 | 　  |
| 1 || SPHRファイル名 | long   long int |    | 〇 | タイムスタンプ  |

### 血圧

*引数*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|  | 物理名   | 論理名 | 型  | 桁数 | 〇=必須/●=非必須 | 　|
| 1 | recordDate   | 測定日時   | long   long int |    | 〇 | タイムスタンプとして連携して頂く。 |
| 2 | modality | 測定方法   | int |    | 〇 | 0:sensed　1:self-reported  |
| 3 | systolic | 最高血圧   | int | 　  | 〇 | 　 |
| 4 | diastolic| 最低血圧   | int | 　  | 〇 | 　 |
| 5 | bodyPosture  | 姿勢   | int | 　  | 〇 | -1:連携無し　0:sitting（座っている）　2:lying   down（横になっている）　3:standing（立っている）　4:semi-recumbent（半横臥位）  |
| 6 | measurementLocation  | 測定位置   | int |    | 〇 | -1:連携無し　0:left ankle（左足首）　1:right   ankle（右足首）　2:lefthip（左腰）　3:righthip（右腰）　4:leftthigh（左ふともも）　5:rightthigh（右ふともも）　6:leftthorax（左胸部）　7:middleleftthorax（左胸部中段）　8:leftupperarm（左上腕）　9:rightupperarm（右上腕）　10:leftwrist（左手首）　11:rightwrist（右手首） |
| 7 | temporalRelationship | 身体活動との時間的関係 | int |    | 〇 | -1:連携無し　0: at rest（安静時）　1:active（活動時）　2: before   exercise（運動前）　3: after exercise（運動後）　4:during exercise（運動中） |

*戻り値*

| No | 項目名 |  | 定義 |  | 出現 | 説明 |
|---|---|---|---|---|---|---|
|  | 物理名  | 論理名| 型 | 桁数 | 〇=必須/●=非必須 | 　  |
| 1 | 　  |SPHRファイル名| long long int |　| 〇|タイムスタンプ|


## #6 エラー定義

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

