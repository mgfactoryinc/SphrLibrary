# PHRデータ交換標準化プロジェクト

経済産業省の予算で動いている PHR データ交換標準化プロジェクトの一部。JSON で出力された PHR データ (SPHR 仕様) を Java オブジェクトにデシリアライズするライブラリを作成する。

## 想定動作環境

- Azure AppService
  - Java 8 SpringBoot (tomcat) Windows

## 想定開発環境

- Windows 10
- JDK 8.0.432+6
- STS (Eclipse Spring Tool Suite) 4.27.0

## 想定成果物

- Web / コンソールアプリに限らず汎用的に利用できる API を `jar` ファイルとして作成し、実行方法は先方に任せる
- 動作確認証明としてユニットテストもある程度実装しておく

## 実装環境

- Windows 11
- OpenJDK 21.0.5 LTS
  - Gradle などの開発環境を動かすランタイムとして利用。
  - 成果物のビルドは JDK 8 で実施する
- VSCode
  - 各種 Java 系プラグインが充実している
  - 慣れない環境の中、 GitHub Copilot の恩恵が大きい
- Gradle 8.12
  - Gradle プロジェクトであれば、Eclipse でインポート可能

### 利用している主なライブラリ

- `jackson-databind`
  - JSON - Java Entity のオブジェクトマッピング・デシリアライザー
  - デシリアライズ時の JSON キー名 - Java Entity プロパティ名のマッピング規則設定
- `lombok`
  - C# と比較して機能不足と言わざるを得ない Java 言語仕様を補う便利ライブラリ
    - getter/setter や各種コンストラクタ、`toString` メソッドなどをコンパイル時に自動生成してくれる各種アノテーションセット
    - Kotlin の `val` 変数に相当する型推論変数
  - Gradle プラグインとして利用している