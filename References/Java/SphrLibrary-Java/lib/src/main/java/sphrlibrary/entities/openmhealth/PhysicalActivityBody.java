package sphrlibrary.entities.openmhealth;

import com.fasterxml.jackson.databind.PropertyNamingStrategies;
import com.fasterxml.jackson.databind.annotation.JsonNaming;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.NonNull;
import lombok.ToString;

/**
 * 歩数データの Body
 */
@Data
@NoArgsConstructor
@ToString(callSuper = true)
@EqualsAndHashCode(callSuper = true)
@JsonNaming(PropertyNamingStrategies.SnakeCaseStrategy.class)
public class PhysicalActivityBody extends OmhBodyEntityBase {

    /**
     * その人が従事している身体活動の名称。
     */
    @NonNull
    private String activityName = "";

    /**
     * 距離
     */
    @NonNull
    private ValueUnit distance = new ValueUnit();

    /**
     * 身体活動の基本動作の繰り返し回数。活動が歩行の場合、base_movement_quantityは歩数となる。
     */
    @NonNull
    private ValueUnit baseMovementQuantity = new ValueUnit();
}
