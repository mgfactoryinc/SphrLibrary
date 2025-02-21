package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * Open mHealth の Body Entity の基底クラス
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public abstract class OmhBodyEntityBase {

    /**
     * 取得時刻 or 期間
     */
    @NonNull
    private TimeFrame effectiveTimeFrame = new TimeFrame();

}
