package sphrlibrary.entities.openmhealth;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.ToString;

/** 血圧データ */
@Data
@NoArgsConstructor
@ToString(callSuper = true)
@EqualsAndHashCode(callSuper = true)
public class BloodPressure extends OmhEntityBase<BloodPressureBody> {

}
