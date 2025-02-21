package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * 外部データシート
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class ExternalDataSheet {

    /** 参照されたデータシートによって記載または文書化された構成要素のタイプ（ソフトウェア、ハードウェア、研究など） */
    @NonNull
    private String datasheetType = "";

    /** 該当するデータシートの国際資源識別子（IRI）。IRIは、同定された資源の場所解決とアクセス情報を伝えることが期待される */
    @NonNull
    private String datasheetReference = "";
}
