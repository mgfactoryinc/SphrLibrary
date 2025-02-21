package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.NonNull;

/**
 * Open mHealth での 測定値と単位
 */
@Data
@NoArgsConstructor
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class ValueUnit {

    /**
     * 測定値
     */
    private double value = 0.0;

    /**
     * 単位
     */
    @NonNull
    private String unit = "";
}
