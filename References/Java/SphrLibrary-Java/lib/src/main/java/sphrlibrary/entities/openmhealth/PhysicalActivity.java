package sphrlibrary.entities.openmhealth;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.ToString;

/**
 * 歩数データ
 */
@Data
@NoArgsConstructor
@ToString(callSuper = true)
@EqualsAndHashCode(callSuper = true)
public class PhysicalActivity extends OmhEntityBase<PhysicalActivityBody> {

}
