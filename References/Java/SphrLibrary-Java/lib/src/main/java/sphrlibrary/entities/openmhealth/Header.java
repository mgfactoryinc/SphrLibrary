package sphrlibrary.entities.openmhealth;

import java.util.ArrayList;
import java.util.List;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * Open mHealth の Header Entity
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class Header {

    /** このデータポイントの識別子：グローバルに一意な値で、RFC 4122の手法で生成される。 */
    @NonNull
    private String uuid = "";

    /**
     * このデータポイントまたはデータ系列が元のソースで作成された日付時間（タイムスタンプ）。データポイントまたはデータ系列が後で集計された場合、日付は変更されない。データポイントまたはデータ系列が後で再計算された場合（例えば、異なるアルゴリズムを使用して）、
     * 異なる ID と作成日時で新しいデータポイントまたはデータ系列が作成される。
     */
    @NonNull
    private String sourceCreationDateTime = "";

    /** データポイントまたはデータ系列の本体のスキーマ識別子 */
    @NonNull
    private SchemaId schemaID = new SchemaId();

    /** 測定結果の取得頻度 */
    @NonNull
    private ValueUnit acquisitionRate = new ValueUnit();

    /**
     * このデータポイントまたはデータ系列の収集、計算、使用などの記載に関連するコンポーネントに関する外部文書（ソフトウェア、アルゴリズム、研究プロトコルなど）への参照（複数可）。
     */
    @NonNull
    private List<ExternalDataSheet> externalDataSheets = new ArrayList<>();

    /** 測定結果を取得したモダリティ（例：測定機器か自己報告か）。 */
    @NonNull
    private String modality = "";
}
