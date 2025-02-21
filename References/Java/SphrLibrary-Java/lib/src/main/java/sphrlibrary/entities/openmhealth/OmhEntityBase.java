package sphrlibrary.entities.openmhealth;

import java.util.ArrayList;
import java.util.List;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * Open mHealth の Entity の基底クラス
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public abstract class OmhEntityBase<TBody extends OmhBodyEntityBase> {

    /** ヘッダー系列のデータ */
    @NonNull
    private Header header = new Header();

    /** ボディ系列のデータ */
    @NonNull
    private List<TBody> body = new ArrayList<>();

    /**
     * データが有効かどうかを返す
     * 
     * @param isCheckAny ボディ系列のデータが 1 つ以上あるかどうかをチェックするかどうか
     * @return データが有効かどうか
     */
    public boolean isValid(boolean isCheckAny) {
        return header != null
                && body != null
                && (isCheckAny ? body.size() > 0 : true);
    }

}
