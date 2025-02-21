package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * 時間間隔
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class TimeInterval {

    /**
     * 開始日時
     */
    @NonNull
    private String startDateTime = "";

    /**
     * 終了日時
     */
    @NonNull
    private String endDateTime = "";
}
