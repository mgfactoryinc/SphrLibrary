# SPHR汎用モジュールのビルド手順

## 要件

Dockerがインストールされ、動作している必要があります。

## ビルド構成の変更

`SphrLibrary.csproj`に以下の変更を加えます。

以下の行を削除してください。
```diff
14,15d13
<!-- 追加のCPU限定命令を有効に出来るオプション -->
<IlcInstructionSet>native</IlcInstructionSet>
```

## 必要なファイルの作成

以下のファイルを新規作成します。

### Dockerfile

```text:Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build

RUN apt-get update \
    && apt-get install -y \
        clang \
        zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /source

COPY --link *.csproj .
RUN dotnet restore -r linux-x64

COPY --link . .
RUN dotnet publish --no-restore -o /out -r linux-x64 SphrLibrary.csproj

FROM scratch
COPY --link --from=build /out /out
```

### .dockerignore

```text:.dockerignore
# directories
**/bin/
**/obj/
**/out/

# files
Dockerfile*
**/*.trx
**/*.md
**/*.ps1
**/*.cmd
**/*.sh
```

### Makefile

```make:Makefile
build: out/out/SphrLibrary.so

out/out/SphrLibrary.so:
	docker build --output ./out .

clean:
	rm -rf out

.PHONY: build clean
```

## プロジェクト構成

プロジェクトディレクトリの構成は以下の通りです。

```text
.
├── Attributes
├── Dockerfile
├── Entities
│   ├── FHIR
│   │   ├── Attachment.cs
│   │   ├── Author.cs
│   │   ├── AuthorInner.cs
│   │   ├── BackboneElement.cs
│   │   ├── CodableConcept.cs
│   │   ├── Coding.cs
│   │   ├── CodingInner.cs
│   │   ├── Composition.cs
│   │   ├── Contained.cs
│   │   ├── DocumentManifest.cs
│   │   ├── DocumentReference.cs
│   │   ├── Fhir.cs
│   │   ├── Identifier.cs
│   │   ├── Organization.cs
│   │   └── Reference.cs
│   ├── OpenmHealth
│   │   ├── BloodPressureBody.cs
│   │   ├── BloodPressure.cs
│   │   ├── BodyTemperature.cs
│   │   ├── BodyWeight.cs
│   │   ├── ExternalDataSheet.cs
│   │   ├── Header.cs
│   │   ├── OmhBodyEntityBase.cs
│   │   ├── OmhEntityBase.cs
│   │   ├── OxygenSaturation.cs
│   │   ├── PhysicalActivityBody.cs
│   │   ├── PhysicalActivity.cs
│   │   ├── SchemaId.cs
│   │   ├── TimeFrame.cs
│   │   ├── TimeInterval.cs
│   │   └── ValueUnit.cs
│   └── SPHR
│       ├── IndexPhr.cs
│       ├── SphrLibrarySettings.cs
│       ├── SphrProfile.cs
│       └── SphrResult.cs
├── Enums
│   ├── BloodPressureMeasurementLocationTypeEnum.cs
│   ├── BodyPostureTypeEnum.cs
│   ├── BodyTemperatureMeasurementLocationTypeEnum.cs
│   ├── CompositionStatusTypeEnum.cs
│   ├── DocumentReferenceTypeEnum.cs
│   ├── DocumentStatusTypeEnum.cs
│   ├── ErrorCode
│   │   ├── SphrClassTypeEnum.cs
│   │   ├── SphrExportErrorTypeEnum.cs
│   │   ├── SphrGeneralErrorTypeEnum.cs
│   │   └── SphrImportErrorTypeEnum.cs
│   ├── ModalityTypeEnum.cs
│   ├── ResourceTypeEnum.cs
│   ├── TemporalRelationshipToPhysicalActivityTypeEnum.cs
│   └── TemporalRelationshipToSleepTypeEnum.cs
├── Extensions
│   ├── ObjectTypeConverterExtension.cs
│   └── StringConverterExtension.cs
├── Helpers
│   ├── FileIOHelper.cs
│   ├── JsonSchemaValidator.cs
│   ├── LogHelper.cs
│   ├── SphrConst.cs
│   ├── SphrHelper.cs
│   ├── SphrJsonSerializer.cs
│   ├── SphrWrapper.cs
│   └── ZipHelper.cs
├── IO
├── Makefile
├── Properties
│   └── PublishProfiles
│       ├── FolderProfile.pubxml
│       └── FolderProfile.pubxml.user
├── Schemas
├── SphrLibrary.cs
├── SphrLibrary.csproj
├── SphrLibrary.csproj.user
├── SphrUnmanagedLibrary.cs
└── Workers
    ├── Args
    │   ├── SphrExportWorkerArgs.cs
    │   └── SphrImportWorkerArgs.cs
    ├── Base
    │   ├── SphrWorkerArgsBase.cs
    │   ├── SphrWorkerBase.cs
    │   └── SphrWorkerResultsBase.cs
    ├── Results
    │   ├── SphrExportWorkerResults.cs
    │   └── SphrImportWorkerResults.cs
    ├── SphrExportWorker.cs
    └── SphrImportWorker.cs
```

## ビルド

以下のコマンドを使用してビルドを実行します。

```shell
make build
```

Dockerを使用してビルドが行われ、`./out/out` ディレクトリに成果物が生成されます。

`SphrLibrary.so` が作成されていることを確認してください。
