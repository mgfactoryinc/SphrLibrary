package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.NonNull;
import lombok.ToString;

/** 血圧データの body */
@Data
@NoArgsConstructor
@ToString(callSuper = true)
@EqualsAndHashCode(callSuper = true)
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class BloodPressureBody extends OmhBodyEntityBase {

    /** 最高血圧 */
    @NonNull
    private ValueUnit systolicBloodPressure = new ValueUnit();

    /** 最低血圧 */
    @NonNull
    private ValueUnit diastolicBloodPressure = new ValueUnit();

    /** 測定時の身体の姿勢 */
    @NonNull
    private String bodyPosture = "";

    /** 測定位置 */
    @NonNull
    private String measurementLocation = "";

    /** 身体活動との時間的関係 */
    @NonNull
    private String temporalRelationshipToPhysicalActivity = "";

    /** 統計情報 */
    @NonNull
    private String descriptiveStatistic = "";
}
