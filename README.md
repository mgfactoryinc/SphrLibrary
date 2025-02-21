# SPHR汎用モジュール
生涯にわたるPHRの管理のため、国民一人一人がPHRサービスを選択し、他のサービスへデータの移動・活用できる「データポータビリティ」を確保することを目的とした、<br>
- 令和６年度ヘルスケア産業基盤高度化推進事業
- 医療機関におけるＰＨＲ利活用推進等に向けた実証調査事業
  
上記プロジェクトの成果物として作成されたPHR情報の事業者間連携を行うためのモジュールです。（以下汎用モジュールと呼称します）

## 汎用モジュールの概要
汎用モジュールは出力するPHR情報と、過去にインポートしたファイルがあればそれをまとめて.sphr拡張子を持つ圧縮ファイルを生成します。<br>
インポートを行う事業者は受け取った圧縮ファイルを汎用モジュールに渡すことで、自社サービスへの取り込みを容易に行うことができます。<br>
各事業者は自社サービスの情報項目を、標準形式へ変換することを意識せずに情報連携を行うことができます。<br>
汎用モジュールはNativeAOT技術を利用し、ワンソースで様々な環境で動作するように作成されています。

## この文書について
汎用モジュールの自社サービスへの組み込み手順を、**事業者エンジニア**に向け解説します。<br>

### 使用技術一覧
<img src="https://img.shields.io/badge/Csharp-000.svg?style=for-the-badge">
<img src="https://img.shields.io/badge/nativeaot-000.svg?style=for-the-badge">


## 汎用モジュールで連携が可能な情報と形式
現在、以下の情報の連携が可能です。

|   | 標準形式 | バージョン |
| ------------- | ------------- | ------------- |
| 血圧  | Open mHealth  |  omh-blood-pressure-4.0  |
| 歩数  | IEEE1752  |  ieee-physical-activity-1.0  | 


### インポートを行う事業者さま
<a href="https://github.com/mgfactoryinc/SphrLibrary/blob/master/Documents/import.md">インポートを行う場合の組み込み手順</a>

### エクスポートを行う事業者さま
<a href="https://github.com/mgfactoryinc/SphrLibrary/blob/master/Documents/export.md">エクスポートを行う場合の組み込み手順</a>


