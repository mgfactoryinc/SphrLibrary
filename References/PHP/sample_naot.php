<?php
/*
  SPHR 汎用モジュール 組み込みサンプル

  PHP(7.4 or later)のソースからコンパイルが必要です
  ./configure --with-ffi

  以下のパスに汎用モジュール用の設定ファイル(.json)を置いてください
  /sphr/SphrLibrarySettings.json
  
  中身は
  {"ServiceId":"com.system.service-a", "ServiceName":"サービスA", "StorageRootPath":"/sphr", "UserIdDigits":"12"}

  ServiceId		サービスID（サービスドメインの逆順。iOS,AndroidアプリのBundleIDと同じ要領）
  ServiceName		サービス名
  StorageRootPath	汎用モジュールが作業するフォルダのルートパス（実行ユーザーでのアクセス権が必要）
  UserIdDigits		サービサーで管理されている利用者を一意に識別するIDの最大桁数（0詰めするため）
*/

// FFI共有ライブラリ(.net 8.0自己完結型)を動的に読み込む
$ffi = FFI::cdef('
void Version();
bool Init(int userId);
bool SetBloodPressureSample();
bool SetPhysicalActivitySample();
bool SetBloodPressure(long long int recordDate, int modality, int systolic, int diastolic, int bodyPosture, int measurementLocation, int temporalRelationship);
bool SetPhysicalActivity(long long int recordDate, int modality, float steps, float distanceKm);
long long int Export();
', __DIR__ . '/lib/SphrLibraryNAOT_linux-x64.so'); //document_rootの下にlibフォルダを切って.soファイルを置いています

// モジュールの読み込み確認
//print $ffi->Version()."\n";
$ffi->Version();

// モジュールの初期化（サービサーの利用者を一意に識別するIDを指定。数字のみ）
$ffi->Init(1234567890);

// 血圧データのセット（サンプル用。実データ用は別途I/F提供予定）
$bp_result = $ffi->SetBloodPressureSample();
print "*** blood pressure result:".$bp_result."\n\n";

// 歩数データのセット（サンプル用。実データ用は別途I/F提供予定）
$pa_result =  $ffi->SetPhysicalActivitySample();
print "*** physical activity result:".$pa_result."\n\n";

// エクスポート タイムスタンプ(ミリ秒3桁)が返ってくる
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
