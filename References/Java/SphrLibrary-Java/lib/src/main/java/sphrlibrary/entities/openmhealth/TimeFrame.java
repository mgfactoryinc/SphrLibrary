package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;

/**
 * 時間枠
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class TimeFrame {

    /**
     * 日付時刻
     */
    private String dateTime = null;

    /**
     * 時間間隔
     */
    private TimeInterval timeInterval = null;
}
